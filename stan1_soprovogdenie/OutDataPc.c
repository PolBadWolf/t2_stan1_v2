
#include "OutDataPc.h"
#include "main.h"

// ================================================
// передача на PC разбита на 3 вида пакетов :
// 1) : передача непосредственно данных с дефектоскопа без 
//      сопровождения - когда стан стоит
//
// 2) : передача байта данных с дефектоскопа сопровожденных до линии реза
//      по условия :
//         1. раз в 5 импульсов колеса
//         2. перед сигналом рез, чтобы не потерять "хвост"
//         3. при простое более 5 минут.
//
// 3) : сообщение о событии окончании старой и начало новой трубы
//      передается по сигналу рез
//
// ======================================================
// структура пакетов :
// 1) 0xE6 0x19 0xFF заголовок
//    0x08           длина пакета (включая контрольную сумму)
//    0x01           вид пакета - состояние дефектоскопов при простое стана
//    0xXX           байт состояния дефектов
//    0x00           выравнивание длины
//    0xCRC          контрольная сумма
//    0x00 0x00 0x00 окончание пакета
//
// 2) 0xE6 0x19 0xFF заголовок
//    0x08           длина пакета (включая контрольную сумму)
//    0x02           вид пакета - сегмент трубы
//    0xNN           номер сегмента по раскладке трубы
//    0xXX           байт состояния дефектов
//    0xCRC          контрольная сумма
//    0x00 0x00 0x00 окончание пакета
//
// 3) 0xE6 0x19 0xFF заголовок
//    0x08           длина пакета (включая контрольную сумму)
//    0x03           вид пакета - новая труба
//    0xDL           длина трубы в сегментах ( один сегмент = 5 импульсов колеса )
//    0x00           выравнивание длины
//    0xCRC          контрольная сумма
//    0x00 0x00 0x00 окончание пакета
//

// счетчик тиков для передачи раз в 1 секунду состояния дефектоскопов при простое стана
unsigned int OutPC_tik1 = 0;

// накоплеление дефектов при работающем стане
unsigned char OutPC_StanOn_Def = 0;

// флаг необходимости передачи дефекта, стан включен
unsigned char OutPC_StanOn_FlSendDef = 0;

// флаг необходимости передачи рез, стан включен
unsigned char OutPC_StanOn_FlSendRez = 0;

// счетчик импульсов колеса на каждый 5 
unsigned char OutPC_StanOn_KolesoCount = 0;
// счетчик сегментов трубы
unsigned char OutPC_StanOn_SegmentCount = 0;

void OutPC_pak1(unsigned char st_def)
{
  unsigned char s_buf[12] = {0xE6,0x19,0xFF,
                             0x08,
                             0x01,
                             0,
                             0,
                             0,
                             0,0,0};
  s_buf[5] = st_def;
  s_buf[7] = crc8_buf(s_buf, 7);
  for (int i=0;i<11;i++ )
    rspc_WriteByte(s_buf[i]);
}

void OutPC_pak2(unsigned char n_seg, unsigned char def_seg)
{
  unsigned char s_buf[12] = {0xE6,0x19,0xFF,
                             0x08,
                             0x02,
                             0,
                             0,
                             0,0,0};
  s_buf[5] = n_seg;
  s_buf[6] = def_seg;
  s_buf[7] = crc8_buf(s_buf, 7);
  for (int i=0;i<11;i++ )
    rspc_WriteByte(s_buf[i]);
}

void OutPC_pak3(unsigned char dl_seg)
{
  unsigned char s_buf[12] = {0xE6,0x19,0xFF,
                             0x08,
                             0x03,
                             0,
                             0,
                             0,
                             0,0,0};
  s_buf[5] = dl_seg;
  s_buf[7] = crc8_buf(s_buf, 7);
  for (int i=0;i<11;i++ )
    rspc_WriteByte(s_buf[i]);
}

void OutPC_tik1_StanStop1()
{
  if (work_tr==255)
  {
    // стан работает
    OutPC_tik1 = 0;
  }
  else
  {
    // стан стоит
    OutPC_tik1++;
  }
}

void OutPC_tik1_StanStop2()
{
  static unsigned char def = 0;
  if (work_tr==0) { // если стан стоит
    unsigned char fl = 0;
    unsigned char cs = __save_interrupt();
    __disable_interrupt();
    if (OutPC_tik1>999)
    {
      OutPC_tik1 = 0;
      fl = 1;
    }
    else
    {
      if (defect_fg_1==255) def |= 1<<0;  // кельвин
      if (defect_fg_2==255) def |= 1<<1;  // B2-1 МД-32 дефект 1
      if (defect_fg_3==255) def |= 1<<2;  // B2-3 МД-32 отказ
      //if (defect_fg_4==255) def |= 1<<3;  // B4 стык PC1
      if (defect_fg_5==255) def |= 1<<4;  // B2-2 от толщиномера "больше"
      if (defect_fg_6==255) def |= 1<<5;  // B3 от толщиномера "меньше"
      if (defect_fg_7==255) def |= 1<<6;
      if (defect_fg_8==255) def |= 1<<7;
    }
    __restore_interrupt(cs);
    if (fl)
    {
      OutPC_pak1(def);
      def = 0;
    }
  }
}

void OutPC_CountImpKoleso()
{
  if (d_koleso_fg2==255)
  {
    d_koleso_fg2 = 0; // сработал импульс колеса
    unsigned char cs = __save_interrupt();
    __disable_interrupt();
    OutPC_StanOn_Def |= d_def[ indx_norm(0) ];
    __restore_interrupt(cs);
    if (work_tr==255) OutPC_StanOn_KolesoCount++;
    if (OutPC_StanOn_KolesoCount>=5)
    {
      OutPC_StanOn_KolesoCount = 0;
      OutPC_StanOn_FlSendDef = 1;
    }
  }
  // проверка на рез
  if (rez_fg1==255)
  {
    rez_fg1 = 0;
    OutPC_StanOn_FlSendRez = 1;
    if (OutPC_StanOn_KolesoCount>0)
    {
      OutPC_StanOn_FlSendDef = 1;
    }
  }
  // передача дефектов
  if (OutPC_StanOn_FlSendDef>0)
  {
    OutPC_StanOn_FlSendDef = 0;
    OutPC_pak2(OutPC_StanOn_SegmentCount, OutPC_StanOn_Def);
    OutPC_StanOn_Def = 0;
    OutPC_StanOn_SegmentCount++;
  }
  // передача был рез
  if (OutPC_StanOn_FlSendRez>0)
  {
    OutPC_StanOn_FlSendRez = 0;
    OutPC_pak3(OutPC_StanOn_SegmentCount);
    OutPC_StanOn_SegmentCount = 0;
    OutPC_StanOn_KolesoCount = 0;
  }
}

void OutPC_int()
{
  // счет тиков для передачи пока стан стоит
  OutPC_tik1_StanStop1();
}

void OutPC_FromMainCycle()
{
  // передача состояния дефектоскопов во время простоя
  OutPC_tik1_StanStop2();
  OutPC_CountImpKoleso();
}

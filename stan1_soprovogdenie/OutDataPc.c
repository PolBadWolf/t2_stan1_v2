
#include "OutDataPc.h"
#include "main.h"

// ================================================
// �������� �� PC ������� �� 3 ���� ������� :
// 1) : �������� ��������������� ������ � ������������ ��� 
//      ������������� - ����� ���� �����
//
// 2) : �������� ����� ������ � ������������ �������������� �� ����� ����
//      �� ������� :
//         1. ��� � 5 ��������� ������
//         2. ����� �������� ���, ����� �� �������� "�����"
//         3. ��� ������� ����� 5 �����.
//
// 3) : ��������� � ������� ��������� ������ � ������ ����� �����
//      ���������� �� ������� ���
//
// ======================================================
// ��������� ������� :
// 1) 0xE6 0x19 0xFF ���������
//    0x08           ����� ������ (������� ����������� �����)
//    0x01           ��� ������ - ��������� ������������� ��� ������� �����
//    0xXX           ���� ��������� ��������
//    0x00           ������������ �����
//    0xCRC          ����������� �����
//    0x00 0x00 0x00 ��������� ������
//
// 2) 0xE6 0x19 0xFF ���������
//    0x08           ����� ������ (������� ����������� �����)
//    0x02           ��� ������ - ������� �����
//    0xNN           ����� �������� �� ��������� �����
//    0xXX           ���� ��������� ��������
//    0xCRC          ����������� �����
//    0x00 0x00 0x00 ��������� ������
//
// 3) 0xE6 0x19 0xFF ���������
//    0x08           ����� ������ (������� ����������� �����)
//    0x03           ��� ������ - ����� �����
//    0xDL           ����� ����� � ��������� ( ���� ������� = 5 ��������� ������ )
//    0x00           ������������ �����
//    0xCRC          ����������� �����
//    0x00 0x00 0x00 ��������� ������
//

// ������� ����� ��� �������� ��� � 1 ������� ��������� ������������� ��� ������� �����
unsigned int OutPC_tik1 = 0;

// ������������ �������� ��� ���������� �����
unsigned char OutPC_StanOn_Def = 0;

// ���� ������������� �������� �������, ���� �������
unsigned char OutPC_StanOn_FlSendDef = 0;

// ���� ������������� �������� ���, ���� �������
unsigned char OutPC_StanOn_FlSendRez = 0;

// ������� ��������� ������ �� ������ 5 
unsigned char OutPC_StanOn_KolesoCount = 0;
// ������� ��������� �����
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
    // ���� ��������
    OutPC_tik1 = 0;
  }
  else
  {
    // ���� �����
    OutPC_tik1++;
  }
}

void OutPC_tik1_StanStop2()
{
  static unsigned char def = 0;
  if (work_tr==0) { // ���� ���� �����
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
      if (defect_fg_1==255) def |= 1<<0;  // �������
      if (defect_fg_2==255) def |= 1<<1;  // B2-1 ��-32 ������ 1
      if (defect_fg_3==255) def |= 1<<2;  // B2-3 ��-32 �����
      //if (defect_fg_4==255) def |= 1<<3;  // B4 ���� PC1
      if (defect_fg_5==255) def |= 1<<4;  // B2-2 �� ����������� "������"
      if (defect_fg_6==255) def |= 1<<5;  // B3 �� ����������� "������"
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
    d_koleso_fg2 = 0; // �������� ������� ������
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
  // �������� �� ���
  if (rez_fg1==255)
  {
    rez_fg1 = 0;
    OutPC_StanOn_FlSendRez = 1;
    if (OutPC_StanOn_KolesoCount>0)
    {
      OutPC_StanOn_FlSendDef = 1;
    }
  }
  // �������� ��������
  if (OutPC_StanOn_FlSendDef>0)
  {
    OutPC_StanOn_FlSendDef = 0;
    OutPC_pak2(OutPC_StanOn_SegmentCount, OutPC_StanOn_Def);
    OutPC_StanOn_Def = 0;
    OutPC_StanOn_SegmentCount++;
  }
  // �������� ��� ���
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
  // ���� ����� ��� �������� ���� ���� �����
  OutPC_tik1_StanStop1();
}

void OutPC_FromMainCycle()
{
  // �������� ��������� ������������� �� ����� �������
  OutPC_tik1_StanStop2();
  OutPC_CountImpKoleso();
}

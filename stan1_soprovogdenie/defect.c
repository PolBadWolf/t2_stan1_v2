#include "main.h"
#include "defect.h"


// B1 стыколов келвин
#define defect_pin_1    ( PIND & 1<<5 )

// B2-1 МД-32 дефект 1
#define defect_pin_2    ( PIND & 1<<4 )

// B2-3 МД-32 отказ
#define defect_pin_4    ( PINB & 1<<6 )

// B4 стык PC1
#define defect_pin_6    ( PINB & 1<<4 )

// B2-2 от толщиномера "больше"
#define defect_pin_3    ( PINB & 1<<7 )

// B3 от толщиномера "меньше"
#define defect_pin_5    ( PINB & 1<<5 )

// отключен
// #define defect_pin_7    ( PINC & 1<<1 )
#define defect_pin_7  255

// отключен
// #define defect_pin_8    ( PINC & 1<<0 )
#define defect_pin_8  255

// счетчик для устранения дребезга
unsigned char defect_count_1;
unsigned char defect_count_2;
unsigned char defect_count_3;
unsigned char defect_count_4;
unsigned char defect_count_5;
unsigned char defect_count_6;
unsigned char defect_count_7;
unsigned char defect_count_8;
// верхний предел счетсчика
unsigned char defect_up_1=50;
unsigned char defect_up_2=50;
unsigned char defect_up_3=50;
unsigned char defect_up_4=50;
unsigned char defect_up_5=50;
unsigned char defect_up_6=50;
unsigned char defect_up_7=50;
unsigned char defect_up_8=50;
// тригер состояние датчиков дефектов
unsigned char defect_tr_1;
unsigned char defect_tr_2;
unsigned char defect_tr_3;
unsigned char defect_tr_4;
unsigned char defect_tr_5;
unsigned char defect_tr_6;
unsigned char defect_tr_7;
unsigned char defect_tr_8;

// флаг состояния дефектов
unsigned char defect_fg_1;
unsigned char defect_fg_2;
unsigned char defect_fg_3;
unsigned char defect_fg_4;
unsigned char defect_fg_5;
unsigned char defect_fg_6;
unsigned char defect_fg_7;
unsigned char defect_fg_8;

// массив дефектов до краско отметчиков ( 2048 ячеек кольцо)
unsigned char d_def[2064];
signed int indx_kol;

// массив дефектов от краско отметчиков до линии реза
unsigned char s_def[155];

// адреса растояния до краскоотметчика от дефектоскопов
unsigned int d_def_sm[16];
unsigned int d_rez_sm;

// индикация дефекта от краскоотметчика до реза
unsigned char ind_d_korez;

extern unsigned char ind_def_k;

// анализ сигнала со стыколова кельвин
// длительность сигнала с "кельвина" в импульсах колеса
signed int kelv_count;
// флаг анализа длины сигнала кельвин
unsigned char kelv_flag;

signed int polovina;


unsigned int indx_norm( signed int data_in)
{
  /*
  unsigned int temp;
  temp = indx_kol + data_in;
  return (temp & 2047 );
  */
    signed int temp;
    temp = indx_kol + data_in;
    while (temp>=2048)
        temp -= 2048;
    while (temp<0)
        temp += 2048;
    return temp;
}

void indx_inc(void)
{
    indx_kol++;
    indx_kol = indx_kol & 2047;
}


//=================================
// анализ сигнала с кельвина и укладка маркировки ограниченой длины
// вход от колеса
/*
void kelv_analiz(void)
{
    if ( kelv_flag==0)
    {   // ожидания начала сигнала
        if ( defect_fg_1==255 )
        {   // кельвин сработал
            kelv_flag=255;
            // сброс сщетчика длины
            kelv_count=0;
        }
        else
        { // ожидание работы кельвина
        }
    }
    else
    {   // ожидание  окончания работы кельвина
        if ( defect_fg_1==255 )
        {   // кельвин еще работает
            kelv_count++;
        }
        else
        {   // кельвин закончил, теперь анализ
            kelv_flag=0;
            if ( work_tr==255 )
            {
              extern unsigned char ind_def12[12];
              // unsigned int i;
              unsigned int adr;
              polovina = kelv_count/2;
              adr = d_def_sm[0]-polovina;
              ind_def12[ adr/ind_def_k ]=ind_def_k;
              // маркировка относительно половины ширины импульса
              // for (i=0;i<16;i++)  d_def[ indx_norm(adr-8+i) ] |= 1<<0;
              d_def[ indx_norm(adr) ] |= 1<<0;
            }
        }
    }
}
*/
void kelv_analiz(void)
{
    if ( kelv_flag==0)
    {   // ожидания начала сигнала
        if ( defect_fg_1==255 )
        {   // кельвин сработал
            kelv_flag=255;
            // сброс сщетчика длины
            //kelv_count=0;
            //==================
            unsigned int adr;
            adr = d_def_sm[0];
            for (signed int i=0;i<10;i++)  d_def[ indx_norm(adr+i-5) ] |= 1<<0;
            //==================
        }
        else
        { // ожидание работы кельвина
        }
    }
    else
    {   // ожидание  окончания работы кельвина
        if ( defect_fg_1==255 )
        {   // кельвин еще работает
            //kelv_count++;
        }
        else
        {   // кельвин закончил
            kelv_flag=0;
            if ( work_tr==255 )
            {
              extern unsigned char ind_def12[12];
              // unsigned int i;
              unsigned int adr;
              //polovina = kelv_count/2;
              //adr = d_def_sm[0]-polovina;
              adr = d_def_sm[0];
              ind_def12[ adr/ind_def_k ]=ind_def_k;
              // маркировка относительно половины ширины импульса
              // for (i=0;i<16;i++)  d_def[ indx_norm(adr-8+i) ] |= 1<<0;
              // d_def[ indx_norm(adr) ] |= 1<<0;
              //for (signed int i=0;i<kelv_count;i++)  d_def[ indx_norm(adr+i) ] |= 1<<0;
            }
        }
    }
}

// чтение датчика
unsigned char defect_test_1(void)
{
    return defect_pin_1==0;
}

unsigned char defect_test_2(void)
{
    return defect_pin_2==0;
}
unsigned char defect_test_3(void)
{
    return defect_pin_3==0;
}
unsigned char defect_test_4(void)
{
    return defect_pin_4==0;
}
unsigned char defect_test_5(void)
{
    return defect_pin_5==0;
}
unsigned char defect_test_6(void)
{
    return defect_pin_6==0;
}
unsigned char defect_test_7(void)
{
    return defect_pin_7==0;
}
unsigned char defect_test_8(void)
{
    return defect_pin_8==0;
}

void defect_int(void)
{
    if ( defect_test_1()==0 )
    {
        if ( defect_count_1>0 ) defect_count_1--;
        if ( defect_count_1==0 && defect_tr_1!=0 )
        if ( defect_count_1==0 )
        {
            defect_tr_1=0;
            defect_fg_1=0;
        }
    }
    else
    {
        if ( defect_count_1<defect_up_1 ) defect_count_1++;
        if ( defect_count_1==defect_up_1 && defect_tr_1!=255 )
        {
            defect_tr_1=255;
            defect_fg_1=255;
        }
    }

    if ( defect_test_2()==0 )
    {
        if ( defect_count_2>0 ) defect_count_2--;
        if ( defect_count_2==0 && defect_tr_2!=0 )
        if ( defect_count_2==0 )
        {
            defect_tr_2=0;
            defect_fg_2=0;
        }
    }
    else
    {
        if ( defect_count_2<defect_up_2 ) defect_count_2++;
        if ( defect_count_2==defect_up_2 && defect_tr_2!=255 )
        {
            defect_tr_2=255;
            defect_fg_2=255;
        }
    }
    if ( defect_test_3()==0 )
    {
        if ( defect_count_3>0 ) defect_count_3--;
        if ( defect_count_3==0 && defect_tr_3!=0 )
        if ( defect_count_3==0 )
        {
            defect_tr_3=0;
            defect_fg_3=0;
        }
    }
    else
    {
        if ( defect_count_3<defect_up_3 ) defect_count_3++;
        if ( defect_count_3==defect_up_3 && defect_tr_3!=255 )
        {
            defect_tr_3=255;
            defect_fg_3=255;
        }
    }

    if ( defect_test_4()==0 )
    {
        if ( defect_count_4>0 ) defect_count_4--;
        if ( defect_count_4==0 && defect_tr_4!=0 )
        if ( defect_count_4==0 )
        {
            defect_tr_4=0;
            defect_fg_4=0;
        }
    }
    else
    {
        if ( defect_count_4<defect_up_4 ) defect_count_4++;
        if ( defect_count_4==defect_up_4 && defect_tr_4!=255 )
        {
            defect_tr_4=255;
            defect_fg_4=255;
        }
    }

    if ( defect_test_5()==0 )
    {
        if ( defect_count_5>0 ) defect_count_5--;
        if ( defect_count_5==0 && defect_tr_5!=0 )
        if ( defect_count_5==0 )
        {
            defect_tr_5=0;
            defect_fg_5=0;
        }
    }
    else
    {
        if ( defect_count_5<defect_up_5 ) defect_count_5++;
        if ( defect_count_5==defect_up_5 && defect_tr_5!=255 )
        {
            defect_tr_5=255;
            defect_fg_5=255;
        }
    }

    if ( defect_test_6()==0 )
    {
        if ( defect_count_6>0 ) defect_count_6--;
        if ( defect_count_6==0 && defect_tr_6!=0 )
        if ( defect_count_6==0 )
        {
            defect_tr_6=0;
            defect_fg_6=0;
        }
    }
    else
    {
        if ( defect_count_6<defect_up_6 ) defect_count_6++;
        if ( defect_count_6==defect_up_6 && defect_tr_6!=255 )
        {
            defect_tr_6=255;
            defect_fg_6=255;
        }
    }

    if ( defect_test_7()==0 )
    {
        if ( defect_count_7>0 ) defect_count_7--;
        if ( defect_count_7==0 && defect_tr_7!=0 )
        if ( defect_count_7==0 )
        {
            defect_tr_7=0;
            defect_fg_7=0;
        }
    }
    else
    {
        if ( defect_count_7<defect_up_7 ) defect_count_7++;
        if ( defect_count_7==defect_up_7 && defect_tr_7!=255 )
        {
            defect_tr_7=255;
            defect_fg_7=255;
        }
    }

    if ( defect_test_8()==0 )
    {
        if ( defect_count_8>0 ) defect_count_8--;
        if ( defect_count_8==0 && defect_tr_8!=0 )
        if ( defect_count_8==0 )
        {
            defect_tr_8=0;
            defect_fg_8=0;
        }
    }
    else
    {
        if ( defect_count_8<defect_up_8 ) defect_count_8++;
        if ( defect_count_8==defect_up_8 && defect_tr_8!=255 )
        {
            defect_tr_8=255;
            defect_fg_8=255;
        }
    }

}

// настройка переменных
void defect_init(void)
{
    // зануление масива дефектов
    {
        unsigned int i;
        for(i=0;i<2060;i++)
          d_def[i]=0;
    }
/*
  //  растояние от дефектоскопов до линии реза
// B1 стыколов келвин 57.484-57.603 М  ( 57.5236 )= 1725.71
  d_def_sm[0]=1726;
// B2-1 МД-32 дефект 1 15.425-15.428 М ( 15.425) = 462.77
  d_def_sm[1]=463;
// B2-2 МД-32 дефект 2 15.550
  d_def_sm[2]=463;
// B2-3 МД-32 отказ 15.550
  d_def_sm[3]=463;
// B3 узк отсутствует
  d_def_sm[4]=466;
// B4 стыколов PC1  6.451-6.458 m (6.457 ) = 193.71
  d_def_sm[5]=194;
// не включен
  d_def_sm[6]=7;
// не включен
  d_def_sm[7]=8;
*/
    def_load();
    // настройка фильтра
    defect_up_1=50;
    defect_up_2=50;
    defect_up_3=50;
    defect_up_4=50;
    defect_up_5=50;
    defect_up_6=50;
    defect_up_7=50;
    defect_up_8=50;
    if ( defect_test_1()==0) defect_fg_1=0; else defect_fg_1=255;
    if ( defect_test_2()==0) defect_fg_2=0; else defect_fg_2=255;
    if ( defect_test_3()==0) defect_fg_3=0; else defect_fg_3=255;
    if ( defect_test_4()==0) defect_fg_4=0; else defect_fg_4=255;
    if ( defect_test_5()==0) defect_fg_5=0; else defect_fg_5=255;
    if ( defect_test_6()==0) defect_fg_6=0; else defect_fg_6=255;
    if ( defect_test_7()==0) defect_fg_7=0; else defect_fg_7=255;
    if ( defect_test_8()==0) defect_fg_8=0; else defect_fg_8=255;
    defect_count_1=defect_up_1/2;
    defect_count_2=defect_up_2/2;
    defect_count_3=defect_up_3/2;
    defect_count_4=defect_up_4/2;
    defect_count_5=defect_up_5/2;
    defect_count_6=defect_up_6/2;
    defect_count_7=defect_up_7/2;
    defect_count_8=defect_up_8/2;
    defect_tr_1=128;
    defect_tr_2=128;
    defect_tr_3=128;
    defect_tr_4=128;
    defect_tr_5=128;
    defect_tr_6=128;
    defect_tr_7=128;
    defect_tr_8=128;
}


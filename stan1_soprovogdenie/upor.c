
#include "main.h"
/*
#include "iom128.h"
#include "ina90.h"
*/

// порт датчика наличия трубы
#define d_trub_1  ( PINE & 1<<4 )   // ДТ1 ( ФРСУ ) B8
#define d_trub_2  ( PINC & 1<<7 )   // ДТ2 B11-1
#define d_trub_3  ( PINC & 1<<6 )   // ДТ3 B11-2
#define d_trub_4  ( PINC & 1<<5 )   // ДТ4 B11-3
#define d_trub_5  ( PINC & 1<<4 )   // ДТ5 B11-4

// управление упором
// #define k_upor_on  { DDRF |= (1<<0) | (1<<4); PORTF &= ~((1<<0) | (1<<4)); }
#define k_upor_on  { DDRF |= (1<<0); PORTF &= ~(1<<0); }
// #define k_upor_on  { DDRF = 255; PORTF = 0; }
// #define k_upor_off { DDRF |= (1<<0) | (1<<4); PORTF |=  (1<<0) | (1<<4); }
#define k_upor_off { DDRF |= (1<<0); PORTF |=  (1<<0); }
// #define k_upor_off { DDRF = 255; PORTF |= 255; }

// счетсчик для датчика наличия трубы
unsigned char trub_count_1;
unsigned char trub_count_2;
unsigned char trub_count_3;
unsigned char trub_count_4;
unsigned char trub_count_5;

// верхний предел счетсчика
unsigned char trub_up_1=10;
unsigned char trub_up_2=10;
unsigned char trub_up_3=10;
unsigned char trub_up_4=10;
unsigned char trub_up_5=10;

// тригер состояния наличия трубы
unsigned char trub_tr_1;
unsigned char trub_tr_2;
unsigned char trub_tr_3;
unsigned char trub_tr_4;
unsigned char trub_tr_5;

// флаг сработки датчика наличия трубы
unsigned char trub_fg_1;
unsigned char trub_fg_2;
unsigned char trub_fg_3;
unsigned char trub_fg_4;
unsigned char trub_fg_5;
unsigned char trub_fg_1i;
unsigned char trub_fg_2i;
unsigned char trub_fg_3i;
unsigned char trub_fg_4i;
unsigned char trub_fg_5i;

// регистры на датчиках для сопровождения дефектов трубы 
unsigned char defect1;
unsigned char defect2;
unsigned char defect3;
unsigned char defect4;
unsigned char defect5;
unsigned char defect1r;
unsigned char defect2r;
unsigned char defect3r;
unsigned char defect4r;

void upor_init(void)
{
  defect1=0;
  defect2=0;
  defect3=0;
  defect4=0;
  defect5=0;
  defect1r=0;
  defect2r=0;
  defect3r=0;
  defect4r=0;
  trub_fg_1=128;
  trub_fg_2=128;
  trub_fg_3=128;
  trub_fg_4=128;
  trub_fg_5=128;
  k_upor_off;
}

// чтение датчика
unsigned char trub_test_1(void)
{
  return d_trub_1;
}
unsigned char trub_test_2(void)
{
  return !d_trub_2;
}
unsigned char trub_test_3(void)
{
  return !d_trub_3;
}
unsigned char trub_test_4(void)
{
  return !d_trub_4;
}
unsigned char trub_test_5(void)
{
  return !d_trub_5;
}

// упор вверх
void upor_up(void)
{
  k_upor_on;
}

// упор вниз
void upor_dn(void)
{
  k_upor_off;
}

//===============================================================
void upor_int(void)
{
  // датчик 1
  if ( trub_test_1() )
  {
    // up
    if ( trub_count_1 < trub_up_1 )
    {
      trub_count_1++;
      if ( ( trub_count_1 == trub_up_1 ) && ( trub_tr_1 == 0 ) )
      {
        trub_tr_1=255;
        trub_fg_1=255;
        trub_fg_1i=255;
      }
    }
  }
  else
  {
    // down
    if ( trub_count_1 > 0 )
    {
      trub_count_1--;
      if ( ( trub_count_1 == 0 ) && ( trub_tr_1 == 255 ) )
      {
        trub_tr_1=0;
        trub_fg_1=0;
        trub_fg_1i=0;
      }
    }
  }
  // датчик 2
  if ( trub_test_2() )
  {
    // up
    if ( trub_count_2 < trub_up_2 )
    {
      trub_count_2++;
      if ( ( trub_count_2 == trub_up_2 ) && ( trub_tr_2 == 0 ) )
      {
        trub_tr_2=255;
        trub_fg_2=255;
        trub_fg_2i=255;
      }
    }
  }
  else
  {
    // down
    if ( trub_count_2 > 0 )
    {
      trub_count_2--;
      if ( ( trub_count_2 == 0 ) && ( trub_tr_2 == 255 ) )
      {
        trub_tr_2=0;
        trub_fg_2=0;
        trub_fg_2i=0;
      }
    }
  }
  // датчик 3
  if ( trub_test_3() )
  {
    // up
    if ( trub_count_3 < trub_up_3 )
    {
      trub_count_3++;
      if ( ( trub_count_3 == trub_up_3 ) && ( trub_tr_3 == 0 ) )
      {
        trub_tr_3=255;
        trub_fg_3=255;
        trub_fg_3i=255;
      }
    }
  }
  else
  {
    // down
    if ( trub_count_3 > 0 )
    {
      trub_count_3--;
      if ( ( trub_count_3 == 0 ) && ( trub_tr_3 == 255 ) )
      {
        trub_tr_3=0;
        trub_fg_3=0;
        trub_fg_3i=0;
      }
    }
  }
  // датчик 4
  if ( trub_test_4() )
  {
    // up
    if ( trub_count_4 < trub_up_4 )
    {
      trub_count_4++;
      if ( ( trub_count_4 == trub_up_4 ) && ( trub_tr_4 == 0 ) )
      {
        trub_tr_4=255;
        trub_fg_4=255;
        trub_fg_4i=255;
      }
    }
  }
  else
  {
    // down
    if ( trub_count_4 > 0 )
    {
      trub_count_4--;
      if ( ( trub_count_4 == 0 ) && ( trub_tr_4 == 255 ) )
      {
        trub_tr_4=0;
        trub_fg_4=0;
        trub_fg_4i=0;
      }
    }
  }
  // датчик 5
  if ( trub_test_5() )
  {
    // up
    if ( trub_count_5 < trub_up_5 )
    {
      trub_count_5++;
      if ( ( trub_count_5 == trub_up_5 ) && ( trub_tr_5 == 0 ) )
      {
        trub_tr_5=255;
        trub_fg_5=255;
        trub_fg_5i=255;
      }
    }
  }
  else
  {
    // down
    if ( trub_count_5 > 0 )
    {
      trub_count_5--;
      if ( ( trub_count_5 == 0 ) && ( trub_tr_5 == 255 ) )
      {
        trub_tr_5=0;
        trub_fg_5=0;
        trub_fg_5i=0;
      }
    }
  }
  
}



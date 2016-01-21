
#include "main.h"

#include "eeprom.h"
#include "macros.h"
//#include "defect.h"
//#include "pular.h"

extern unsigned char ind_def_k;

//#define ind_def_kr 1800/12
#define ind_def_kr 1800/12

//----------------------------------------
// 12.08.2008
// работа с 3 краскопультом ( для тощиномера )
//
// переменнные в main.c
// время - длительность импульса покраски
// для переднего конца трубы
extern int kr3_dl_const;
// время задержки  от переднего конца трубы ( м сек ) до заднего
extern int kr3_zd_const;



void ep_zero(void)
{
    unsigned char i;
    for (i=0;i<200;i++)
        WriteEeprom(i,0);
}

void def_save(void)
{
    unsigned char i;
    unsigned int max_dl;
    max_dl=0;
    WriteEeprom( 190, 0x19 );
    for ( i=0;i<8;i++)
    {
        WriteEeprom( i*2+1, LOW(d_def_sm[i]) );
        WriteEeprom( i*2+2, HIGH(d_def_sm[i]) );
        if ( d_def_sm[i]>max_dl ) max_dl = d_def_sm[i];
    }
    // ind_def_k = ( ((max_dl*10)/12)+5 )/10;
    WriteEeprom( 10*2+1,  LOW(d_rez_sm) );
    WriteEeprom( 10*2+2, HIGH(d_rez_sm) );

    // время - длительность импульса покраски
    WriteEeprom( 10*3+0,  LOW(kr3_dl_const) );
    WriteEeprom( 10*3+1, HIGH(kr3_dl_const) );
    // время задержки  от переднего конца трубы
    WriteEeprom( 10*3+2,  LOW(kr3_zd_const) );
    WriteEeprom( 10*3+3, HIGH(kr3_zd_const) );
    // масштаб шкалы
    WriteEeprom( 10*3+4,  LOW(ind_def_k) );
    WriteEeprom( 10*3+5, HIGH(ind_def_k) );
}

void def_load(void)
{
    unsigned char i;
    unsigned int max_dl;
    if ( ReadEeprom( 190 )!=0x19 )
    {
        _CLI();
        //  растояние от дефектоскопов до КО
        // B1 стыколов келвин 
        d_def_sm[0]=1600; // 53.33m
        // B2-1 МД-32 дефект 1 
        d_def_sm[1]=358; // 11.93m
        // B2-3 МД-32 отказ 
        d_def_sm[3]=358;
        // B4 стыколов PC1  
        d_def_sm[5]=78; // 2.6 M

        // Толщиномер
            // B2-2 больше
        // d_def_sm[2]=1290; // 
        d_def_sm[2]=1290; // 
        // B3 меньше
        d_def_sm[4]=1290; 

        // не включен
        d_def_sm[6]=0;
        // не включен
        d_def_sm[7]=0;

        // растояние от КО до линии реза
        d_rez_sm=108;
        // время - длительность импульса покраски
        kr3_dl_const=400;
        // время задержки  от переднего конца трубы
        kr3_zd_const=11000;
        ind_def_k = ind_def_kr;
        _SEI();
        def_save();
    }
    else
    {
        _CLI();
        for ( i=0;i<8;i++)
        {
            LOW(d_def_sm[i]) = ReadEeprom( i*2+1 );
            HIGH(d_def_sm[i]) = ReadEeprom( i*2+2 );
            if ( d_def_sm[i]>max_dl )
                max_dl = d_def_sm[i];
        }
        // ind_def_k = ( ((max_dl*10)/12)+5 )/10;
        LOW(d_rez_sm) = ReadEeprom( 10*2+1 );
        HIGH(d_rez_sm) = ReadEeprom( 10*2+2 );
    
        // время - длительность импульса покраски
        LOW(kr3_dl_const) = ReadEeprom(10*3+0);
        HIGH(kr3_dl_const) = ReadEeprom(10*3+1);
        // время задержки  от переднего конца трубы
        LOW(kr3_zd_const) = ReadEeprom(10*3+2);
        HIGH(kr3_zd_const) = ReadEeprom(10*3+3);
        // масштаб шкалы
        LOW(ind_def_k)    = ReadEeprom(10*3+4);
        HIGH(ind_def_k)    = ReadEeprom(10*3+5);
        _SEI();
    }
}

/*
void kk_save(void)
{
  WriteEeprom( 195, 0x19 );
  WriteEeprom( 64,  LOW(kk_sm[8]) );
  WriteEeprom( 65, HIGH(kk_sm[8]) );
  WriteEeprom( 66,  LOW(kk_sm[9]) );
  WriteEeprom( 67, HIGH(kk_sm[9]) );
}

void kk_load(void)
{
  if ( ReadEeprom( 195 )!=0x19 )
  {
    _CLI();
      //  растояние от дефектоскопов до линии реза
      // КК дефектов 3.5м  105
      kk_sm[8]=105;
      // КК стыков   3.5м 105
      kk_sm[9]=105;
    _SEI();
    kk_save();
  }
  else
  {
    _CLI();
     LOW(kk_sm[8]) = ReadEeprom( 64 );
    HIGH(kk_sm[8]) = ReadEeprom( 65 );
     LOW(kk_sm[9]) = ReadEeprom( 66 );
    HIGH(kk_sm[9]) = ReadEeprom( 67 );
    _SEI();
  }
}
*/


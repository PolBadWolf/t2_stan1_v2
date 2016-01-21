
#include "main.h"

/*
#include "iom128.h"
#include <ina90.h>
*/

// флаг сработки прерывания
unsigned char t_int=0;
// часы для отладки
extern unsigned int t_msec;
extern unsigned char t_sec;
extern unsigned char t_min;
extern unsigned char t_chas;
// ошибка
extern unsigned int error_code;
extern unsigned int error_data;

// цикл колесо
extern unsigned char koleso_cicle;

// время между импульсами колеса в милисекундах
extern unsigned int speed_x;
extern unsigned int speed_int;
extern unsigned int speed_usr[5];
extern unsigned char speed_n;


/*
#include "lcd_16.h"
#include "defect.h"
#include "koleso.h"
#include "rez.h"
#include "upor.h"
#include "pular.h"
*/
//kp3
void int_kr3(void);

// =========================================================
// = обработка прерывания от Timer 0                       =
// =========================================================
#pragma vector = TIMER0_COMP_vect /* Прерывание 1000 Hz */
__interrupt void tim0_interrupt(void)
{
  t_int++;
  _SEI();
  if ( t_int>1 ) error_code=2;
  lcd_int();
// часы ------------------------------------
      if ( ++t_msec==1000 )
      {
        t_msec=0;
        if ( ++t_sec==60 )
        {
          t_sec=0;
          if ( ++t_min==60 )
          {
            t_min=0;
            if ( ++t_chas==24 ) t_chas=0;
          }
        }
      }
  // цифровой фильтр дефектов
  defect_int();
  // цифровой фильтр колеса
  koleso_int();
  // цифровой фильтр сигнала рез
  rez_int();
  // цифровой фильтр для датчиков труб
  upor_int();
  pulsar_int();
  // kr3
  int_kr3(); 
  // debug
  circle_view();
  circle_tah();

  // накопление дефектов
  if ( work_tr==255 )
  {
      extern unsigned char ind_def12[12];
      extern unsigned char ind_def8[8];
      //кельвин ( сейчас особая процедура )
      // if ( defect_fg_1==255 ) d_def[ indx_norm(d_def_sm[0]) ] |= 1<<0;
      // B2-1 МД-32 дефект 1
      if ( defect_fg_2==255 ) { d_def[ indx_norm(d_def_sm[1]) ] |= 1<<1; ind_def8[1]=1; }
      // B2-3 МД-32 отказ
      if ( defect_fg_4==255 ) { d_def[ indx_norm(d_def_sm[3]) ] |= 1<<2; ind_def8[3]=1; }
      // B4 стык PC1
      if ( defect_fg_6==255 ) { d_def[ indx_norm(d_def_sm[5]) ] |= 1<<3; ind_def8[5]=1; }


      // B2-2 от толщиномера "больше"
      if ( defect_fg_3==255 ) { d_def[ indx_norm(d_def_sm[2]) ] |= 1<<4; ind_def8[2]=1; }
      // B3 от толщиномера "меньше"
      if ( defect_fg_5==255 ) { d_def[ indx_norm(d_def_sm[4]) ] |= 1<<5; ind_def8[4]=1; }



      if ( defect_fg_7==255 ) { d_def[ indx_norm(d_def_sm[6]) ] |= 1<<6; ind_def8[6]=1; }
      if ( defect_fg_8==255 ) { d_def[ indx_norm(d_def_sm[7]) ] |= 1<<7; ind_def8[7]=1; }
  }
  //***********************************************
  //      обработка для передачи на компьютер
  OutPC_int();
  // **********************************************
// датчик колесо сработал
      if ( d_koleso_fg==255)
      {
        // сработало колесо
        d_koleso_fg=0;
        koleso_cicle++;
      }

  {
    // time out
    extern unsigned int timeout_key;
    if ( timeout_key>1 )  timeout_key--;
  }
  t_int--;
}

void init_timer0(void)
{
 TCCR0 = 0x0D;  // Коэффициент деления 64, очистка при совпадении
 TCNT0 = 0;     // Fx=Fs/( TCCR0 * ( 1+ OCR0 ) )
 OCR0 = 125; 
 TIMSK |= 1<<OCIE0; // Разрешить прерывания от Timer/Counter 0
}


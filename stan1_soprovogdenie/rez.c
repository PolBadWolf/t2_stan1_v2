
#include "main.h"
#include "rez.h"

// порт датчика отрезания трубы
// B6
#define rez_port  ( PINE & 1<<6 )

// счетсчик для датчика реза
unsigned char rez_count;
// верхний предел счетсчика
unsigned char rez_up=50;
// тригер состояния датчика реза
unsigned char rez_tr;
// флаг сработки датчика реза
unsigned char rez_fg;
unsigned char rez_fg1;

// масивы :

// массив дефектов
unsigned char rez_ms_def[10];
// массив начала трубы
unsigned int rez_ms_ind[10];
// в разах длина от линии реза до датчика
unsigned int rez_end=375;

// настройка - сброс
void rez_init(void)
{
  unsigned char i;
  for (i=0;i<5;i++)
  {
    rez_ms_def[i]=0;
    rez_ms_ind[i]=0;  // rez_end;
  }
  rez_ms_ind[1]=1;
}

// чтение датчика
unsigned char rez_test(void)
{
  return !rez_port;
}

void rez_int(void)
{
  if ( rez_test() )
  {
    // up
    if ( rez_count < rez_up )
    {
      rez_count++;
      if ( ( rez_count == rez_up ) && ( rez_tr == 0 ) )
      {
        rez_tr=255;
        rez_fg=255;
        rez_fg1=255;
      }
    }
  }
  else
  {
    // down
    if ( rez_count > 0 )
    {
      rez_count--;
      if ( rez_count == 0) rez_tr=0;
    }
  }
}


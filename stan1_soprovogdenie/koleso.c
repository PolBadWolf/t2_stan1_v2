
#include "main.h"

// датчик колесо :
// счетчик для устранения дребезга
unsigned char d_koleso_count;
// верхний предел счетсчика
unsigned char d_koleso_up=3;
// тригер состояние датчика колеса
unsigned char d_koleso_tr;
// внешний флаг сработки датчика колеса
unsigned char d_koleso_fg;
// внутренний флаг сработки датчика колеса
unsigned char d_koleso_fg1;
// внутренний флаг сработки датчика колеса для передачи в компьютер
unsigned char d_koleso_fg2;

// порт датчика
// B5 
#define koleso_port  ( PINE & (1<<7) )

// on & off = speed
// состояние в прошлый раз
unsigned char circle_test;
// счетчик
unsigned char circle_count;
// в включеном состоянии
unsigned char circle_count_on;
// в выключеном состоянии
unsigned char circle_count_off;

// счетчик импульсов скорости
// счетчик импульсов времени
unsigned int circle_time;
// счетчик импульсов колеса
unsigned int circle_speed_count;
// скорость
unsigned int circle_speed;


//------------
// debug
unsigned int kolxc;

// чтение датчика
unsigned char koleso_test(void)
{
  return !koleso_port;
}

// отработка датчика колеса
void koleso_int(void)
{
    // датчик колеса - состояние
    if ( koleso_test() )
    {
        // сработал
        //
        // если счетчик не в "потолке"
        if ( d_koleso_count < d_koleso_up )
        {
            // счетчик +1
            d_koleso_count++;
            // если потолок и тригер в состояние датчик не сработан
            if ( (d_koleso_count==d_koleso_up) && (d_koleso_tr==0) )
            {
                // установка тригера датчик сработан
                d_koleso_tr=255;
                // установка флага импульс от колеса
                d_koleso_fg=255;
                d_koleso_fg1=255;
                d_koleso_fg2=255;
            }
        }
    }
    else
    {
        // не сработан
        //
        // если счетчик не в нуле, то -1
        if ( d_koleso_count > 0 )
        {
            d_koleso_count--;
            if ( d_koleso_count==0 )
                d_koleso_tr=0;
        }
    }
    //  return d_koleso_fg;
    if ( d_koleso_tr==255 )
    {
        DDRF |= (1<<5);
        PORTF &= ~(1<<5);
    }
    else
    {
        DDRF |= (1<<5);
        PORTF |=  (1<<5);
    }
}

// on & off
void circle_view(void)
{
    unsigned char temp;
    temp=koleso_test();
    if ( temp==circle_test )
    {
        // подсчет продолжается
        if ( circle_count<99 )
            circle_count++;
        if ( circle_count==99 )
        {
            if ( circle_test==0 )
                circle_count_on=circle_count;
            else
                circle_count_off=circle_count;
        }
    }
    else
    {
        if ( circle_test==0 )
            circle_count_on=circle_count;
        else
            circle_count_off=circle_count;
        circle_test=temp;
        circle_count=1;
    }
}

void circle_tah(void)
{
    if ( d_koleso_fg1==255 )
    {
        d_koleso_fg1=0;
        circle_speed_count++;
    }
    // время
    circle_time++;
    if ( circle_time>1999 )
    {
        circle_speed=circle_speed_count;
        circle_speed_count=0;
        circle_time=0;
    }
}


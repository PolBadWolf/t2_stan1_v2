
#include "main.h"

// ������ ������ :
// ������� ��� ���������� ��������
unsigned char d_koleso_count;
// ������� ������ ���������
unsigned char d_koleso_up=3;
// ������ ��������� ������� ������
unsigned char d_koleso_tr;
// ������� ���� �������� ������� ������
unsigned char d_koleso_fg;
// ���������� ���� �������� ������� ������
unsigned char d_koleso_fg1;
// ���������� ���� �������� ������� ������ ��� �������� � ���������
unsigned char d_koleso_fg2;

// ���� �������
// B5 
#define koleso_port  ( PINE & (1<<7) )

// on & off = speed
// ��������� � ������� ���
unsigned char circle_test;
// �������
unsigned char circle_count;
// � ��������� ���������
unsigned char circle_count_on;
// � ���������� ���������
unsigned char circle_count_off;

// ������� ��������� ��������
// ������� ��������� �������
unsigned int circle_time;
// ������� ��������� ������
unsigned int circle_speed_count;
// ��������
unsigned int circle_speed;


//------------
// debug
unsigned int kolxc;

// ������ �������
unsigned char koleso_test(void)
{
  return !koleso_port;
}

// ��������� ������� ������
void koleso_int(void)
{
    // ������ ������ - ���������
    if ( koleso_test() )
    {
        // ��������
        //
        // ���� ������� �� � "�������"
        if ( d_koleso_count < d_koleso_up )
        {
            // ������� +1
            d_koleso_count++;
            // ���� ������� � ������ � ��������� ������ �� ��������
            if ( (d_koleso_count==d_koleso_up) && (d_koleso_tr==0) )
            {
                // ��������� ������� ������ ��������
                d_koleso_tr=255;
                // ��������� ����� ������� �� ������
                d_koleso_fg=255;
                d_koleso_fg1=255;
                d_koleso_fg2=255;
            }
        }
    }
    else
    {
        // �� ��������
        //
        // ���� ������� �� � ����, �� -1
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
        // ������� ������������
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
    // �����
    circle_time++;
    if ( circle_time>1999 )
    {
        circle_speed=circle_speed_count;
        circle_speed_count=0;
        circle_time=0;
    }
}


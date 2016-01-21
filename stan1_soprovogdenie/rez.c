
#include "main.h"
#include "rez.h"

// ���� ������� ��������� �����
// B6
#define rez_port  ( PINE & 1<<6 )

// �������� ��� ������� ����
unsigned char rez_count;
// ������� ������ ���������
unsigned char rez_up=50;
// ������ ��������� ������� ����
unsigned char rez_tr;
// ���� �������� ������� ����
unsigned char rez_fg;
unsigned char rez_fg1;

// ������ :

// ������ ��������
unsigned char rez_ms_def[10];
// ������ ������ �����
unsigned int rez_ms_ind[10];
// � ����� ����� �� ����� ���� �� �������
unsigned int rez_end=375;

// ��������� - �����
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

// ������ �������
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


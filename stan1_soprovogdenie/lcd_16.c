// ��������� ����128, lcd 16

unsigned char n_str=16;

/*
#include "iom128.h"
#include "ina90.h"
*/
#include "main.h"

#include "comm_def.h"

// ������
unsigned char lcd_display[32]; /* ���������� ����� �� 32 ����� */
unsigned char disp_count=0;  /* ������� ��������� ��������� */

unsigned char lcd_flag;    /* ����� ��� ������ � �������� */
unsigned int blinc_line1;      /* ������� ������ ������� 1 ������ ���������� */
unsigned int blinc_line2;      /* ������� ������ ������� 2 ������ ���������� */

//===========================================================================
//#include <iom128.h>   /* ���� � ������������� ������ � ��������� ���������� */
//#include <ina90.h>
//#include "comm_def.h" /* ���� � ������������� ������ */
//#include "port_def.h" /* ���� � ������������� �������� � �������� */

#define DELAY_REPEAT       700 /* �������� � �� ����� ������������ */
#define TIME_REPEAT        150 /* ������ ����������� */
#define DELAY_BEEP         100 /* ������������ � �� ��������� ������� */

unsigned int flash flash_decoder[32]={
 1,2,4,8,16,32,64,128,256,512,1024,2048,4096,8192,16384,32768
,1,2,4,8,16,32,64,128,256,512,1024,2048,4096,8192,16384,32768
};


/* ���� PB */
#define LCDRS      0x20 /* PORT_OUT, ����� �������� ������/���������� � �� */
#define LCDEN      0x10 /* PORT_OUT, ����� ��� �������� ������ � �� ���������� */
#define LCDEN_ZERO  PORTA &= ~LCDEN /* ������ "0" �� ���� ����������  */
#define LCDEN_ONE   PORTA |= LCDEN  /* ������ "1" �� ���� ���������� */
#define LCD_CONTROL PORTA &= ~LCDRS /* ������� ������� ��������� */
#define LCD_DATA    PORTA |= LCDRS  /* ������� ������� ������ */
#define LOAD_LCD    {LCDEN_ONE;_NOP();_NOP();_NOP();_NOP();LCDEN_ZERO;} /* ��������� ������ � LCD ��������� */
#define PORT_LCD    PORTA   /* ���� ���������� LCD */
#define PORT_LCDKEY  PORTG   /* ���� ���������� ����������  */

#define BELL         0x10 /* PORT_OUT, �������� ������ */
//#define LED        0x02 /* PORT_OUT, ���������  */
#define BELL_ON      PORTG &= ~BELL
#define BELL_OFF     PORTG |= BELL
#define BELL_TOGGLE  PORTG ^= BELL

#define PORT_LCD_DATA_INST    DDRA
#define PORT_LCD_CONTROL_INST DDRA

unsigned char display[32]; /* ���������� ����� �� 32 ����� */
unsigned char disp_count;  /* ������� ��������� ��������� */
unsigned char kbd_count;   /* ������� ��������� ����-���� */
unsigned char time_return;     /* ������� �������� ����������� � ������� ����� */
unsigned char kbd_shift = 0x01;/* ��������� �� ������� ������� ������ */
unsigned char beep_count;      /* ������� ������������ ��������� ������� */
unsigned int  repeat_count;    /* ������� �������� ����������� */
unsigned int  blink_count=500; /* ������� �������� ������� ������� */
unsigned int  sec_recount = 1000; /* ����������� ������� ������ */
unsigned char sys_flags;       /* ��������� ����� ��������� ������� */
unsigned char dispkbd_flag;    /* ����� ��� ������ � �������� � ����������� */
unsigned char scan_code;       /* ������� ����-���� ���������� */
unsigned char keyboard;        /* ������� ������ ��������� ���������� */
unsigned int blinc_line1;      /* ������� ������ ������� 1 ������ ���������� */
unsigned int blinc_line2;      /* ������� ������ ������� 2 ������ ���������� */
unsigned char mig_string=0;

/* uchar sys_flags  ��������� ����� ��������� ������� */
#define F_START_BEEP     0x01 /* ����  */
#define F_SAVE           0x02 /* ���� ���������� ���������� ��������� */
#define F_RETURN_WORK    0x04 /* ���� "������� � ������� �����" */
#define F_SECONDS        0x08 /* ���� "��������� �������"  */
#define F_AVERAG         0x10 /* ���� "����������� �����������" */
#define F_REGIM          0x20 /* ���� "��������� ������" */
#define F_DISPLAY_CLEAR  0x40 /* ���� "������� ������" */
#define F_ALARM          0x80 /* ���� "�������" */

#define SET_START_BEEP    sys_flags |= F_START_BEEP  /* ���������� ���� ������ �������� ��������� ������� */
#define RESET_START_BEEP  sys_flags &= ~F_START_BEEP /* �������� ���� ������ �������� ��������� ������� */
#define START_BEEP       (sys_flags & F_START_BEEP) /* ��������� ���� ������ �������� ��������� ������� */

#define SET_SAVE          sys_flags |= F_SAVE  /* ���������� ���� "��������� ���������" */
#define RESET_SAVE        sys_flags &= ~F_SAVE /* �������� ���� "��������� ���������" */
#define SAVE             (sys_flags & F_SAVE) /* ��������� ���� "��������� ���������" */

#define SET_RETURN_WORK   sys_flags |= F_RETURN_WORK  /* ���������� ���� "������� � ������� �����" */
#define RESET_RETURN_WORK sys_flags &= ~F_RETURN_WORK /* �������� ���� "������� � ������� �����" */
#define RETURN_WORK      (sys_flags & F_RETURN_WORK) /* ��������� ���� "������� � ������� �����" */

#define SET_SECONDS       sys_flags |= F_SECONDS  /* ���������� ���� "��������� �������" */
#define RESET_SECONDS     sys_flags &= ~F_SECONDS /* �������� ���� "��������� �������" */
#define SECONDS          (sys_flags & F_SECONDS) /* ��������� ���� "��������� �������" */

#define SET_AVERAG        sys_flags |= F_AVERAG  /* ���������� ���� "����������� �����������" */
#define RESET_AVERAG      sys_flags &= ~F_AVERAG /* �������� ���� "����������� �����������" */
#define AVERAG           (sys_flags & F_AVERAG) /* �������� ����� "����������� �����������" */

#define START_REGIM       sys_flags |= F_REGIM  /* ���������� ���� "��������� ������" */
#define STOP_REGIM        sys_flags &= ~F_REGIM /* �������� ���� "��������� ������" */
#define REGIM            (sys_flags & F_REGIM) /* ��������� ���� "��������� ������" */

#define SET_ALARM         sys_flags |= F_ALARM  /* ���������� ���� "�������" */
#define RESET_ALARM       sys_flags &= ~F_ALARM /* �������� ���� "�������" */
#define ALARM            (sys_flags & F_ALARM) /* ��������� ���� "�������" */

#define SET_DISPLAY_CLEAR sys_flags |= F_DISPLAY_CLEAR  /* ���������� ���� "������� ������" */
#define RESET_DISPLAY_CLEAR sys_flags &= ~F_DISPLAY_CLEAR /* �������� ���� "������� ������" */
#define DISPLAY_CLEAR    (sys_flags & F_DISPLAY_CLEAR) /* ��������� ���� ������� ������� */

#define SET_UPD_CLOCK     sys_flags |= F_UPD_CLOCK  /* ���������� ���� "�������� ��������� �������" */
#define RESET_UPD_CLOCK   sys_flags &= ~F_UPD_CLOCK /* �������� ���� "�������� ��������� �������" */
#define UPD_CLOCK        (sys_flags & F_UPD_CLOCK) /* �������� ����� "�������� ��������� �������" */

/* uchar dispkbd_flag  ��������� ����� ��������� ���������� � ��������� */
#define F_TWO_LINE       0x01 /* ���� �������� �� ������ ������ ���������� */
#define F_SCAN           0x02 /* ���� ���������� ����-���� */
#define F_SHOW_EN        0x04 /* ���� ���������� ��������� �� ������� */
#define F_BLINK          0x08 /* ���� ������� � �������� 2 �� */
#define F_AUTO_REPEAT    0x10 /* ���� ���������� ����������� ���� ������ */
#define F_START_DELAY    0x20 /* ���� ������ �������� ����������� */
#define F_STOP_DELAY     0x40 /* ���� ���������� �������� ����������� */
#define F_KEYB_REPEAT    0x80 /* ���� �������� �������� ����������� */

#define SET_TWO_LINE     dispkbd_flag |= F_TWO_LINE
#define RESET_TWO_LINE   dispkbd_flag &= ~F_TWO_LINE
#define TWO_LINE         (dispkbd_flag & F_TWO_LINE) /* ��������� ���� �������� �� ������ ������ ���������� */

#define SET_SCAN_READY   dispkbd_flag |= F_SCAN
#define RESET_SCAN_READY dispkbd_flag &= ~F_SCAN
#define SCAN_READY       (dispkbd_flag & F_SCAN) /* ��������� ���� ����-���� */

#define SET_SHOW_EN      dispkbd_flag |= F_SHOW_EN
#define RESET_SHOW_EN    dispkbd_flag &= ~F_SHOW_EN
#define SHOW_EN          (dispkbd_flag & F_SHOW_EN) /* ��������� ���� ���������� ����� ��������� */

#define TOGGLE_BLINK     dispkbd_flag ^= F_BLINK
#define BLINK            (dispkbd_flag & F_BLINK) /* ��������� ���� ������� */

#define SET_AUTO_REPEAT   dispkbd_flag |= F_AUTO_REPEAT  /* ���������� ���� ���������� ����������� */
#define RESET_AUTO_REPEAT dispkbd_flag &= ~F_AUTO_REPEAT /* �������� ���� ���������� ����������� */
#define AUTO_REPEAT       (dispkbd_flag & F_AUTO_REPEAT) /* ��������� ���� ���������� ����������� */

#define SET_START_DELAY   dispkbd_flag |= F_START_DELAY  /* ���������� ���� ������ �������� */
#define RESET_START_DELAY dispkbd_flag &= ~F_START_DELAY /* �������� ���� ������ �������� */
#define START_DELAY       (dispkbd_flag & F_START_DELAY) /* ��������� ���� ���������� ������ �������� */

#define SET_STOP_DELAY    dispkbd_flag |= F_STOP_DELAY  /* ���������� ���� ��������� �������� */
#define RESET_STOP_DELAY  dispkbd_flag &= ~F_STOP_DELAY /* �������� ���� ��������� �������� */
#define STOP_DELAY        (dispkbd_flag & F_STOP_DELAY) /* ��������� ���� ���������� ��������� �������� */

#define SET_KEYB_REPEAT   dispkbd_flag |= F_KEYB_REPEAT  /* ���������� ���� �������� �������� ����������� */
#define RESET_KEYB_REPEAT dispkbd_flag &= ~F_KEYB_REPEAT /* �������� ���� �������� �������� ����������� */
#define KEYB_REPEAT       (dispkbd_flag & F_KEYB_REPEAT) /* ��������� ���� �������� �������� ����������� */


/* ������� ������������� ��� ���������� */
flash unsigned char TableDecode[] = {
0xD9,0xDA,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,
0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,
0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,
0xA2,0xB5,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,0x20,
0x41,0xA0,0x42,0xA1,0xE0,0x45,0xA3,0xA4,0xA5,0xA6,0x4B,0xA7,0x4D,0x48,0x4F,0xA8,
0x50,0x43,0x54,0xA9,0xAA,0x58,0xE1,0xAB,0xAC,0xE2,0xAD,0xAE,0x62,0xAF,0xB0,0xB1,
0x61,0xB2,0xB3,0xB4,0xE3,0x65,0xB6,0xB7,0xB8,0xB9,0xBA,0xBB,0xBC,0xBD,0x6F,0xBE,
0x70,0x63,0xBF,0x79,0xE4,0x78,0xE5,0xC0,0xC1,0xE6,0xC2,0xC3,0xC4,0xC5,0xC6,0xC7};

void ClearDisplay(void) /* �������� ��������� */
{
 if(!DISPLAY_CLEAR) /* ���� ������� �� ��� ������, �������� */
 {
  unsigned char *ptr_1 = &display[0];
  // ��������� ������� ������
  blinc_line1 = 0x0000;
  blinc_line2 = 0x0000;
  for(;ptr_1 <= &display[31];)  *ptr_1++ = 0x20;
  SET_DISPLAY_CLEAR; /* ���������� ������� ������� ������� */
 }
}

void ClearDisplay1(void) /* �������� ��������� */
{
  unsigned char *ptr_1 = &display[0];
  // ��������� ������� ������
  blinc_line1 = 0x0000;
  blinc_line2 = 0x0000;
  for(;ptr_1 <= &display[31];)  *ptr_1++ = 0x20;
  SET_DISPLAY_CLEAR; /* ���������� ������� ������� ������� */
}

/* ������� ������ �� ��������� � �������� ������� */
void Show(unsigned char pos, unsigned char flash* t)
{
 unsigned char *s;
// if(pos < 32)
// {
  s = display + pos;
  while(*t) *s++ = *t++;
// }
}

void LoadLcd(void)  /* ��������� ������ � LCD ��������� */
{
 LCDEN_ONE;
 _NOP();
 _NOP();
 _NOP();
 _NOP();
 LCDEN_ZERO;
}

void Delay4mks(void)
{
 unsigned char i;
 for(i = 17;i;i--);
}

void delay_mks(unsigned int time)
{
 unsigned int counter;
 unsigned int constant;
 constant = time /4;
 for(counter=0;counter < constant;counter++)
  Delay4mks();
}

void InitLcd(void)     /* ������������� ���������� */
{
PORT_LCD_DATA_INST = 0x0f;
PORT_LCD_CONTROL_INST |= ( LCDEN | LCDRS );
DDRG_DDG0 = 0;
DDRG_DDG1 = 0;
DDRG_DDG2 = 0;
DDRG_DDG3 = 0;

delay_mks(30000); //16000
LCD_CONTROL;           /* ������� ������� ���������� */
PORT_LCD = 0x03;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
delay_mks(4600);//4100
PORT_LCD = 0x03;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
delay_mks(4600);//100
PORT_LCD = 0x03;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
delay_mks(4600);//4100
PORT_LCD = 0x02;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
delay_mks(4600);//60
                       /* 4-bit operation, 1/16 duty cycle, font 5x8 dot matrix */
PORT_LCD = 0x02;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
PORT_LCD = 0x08;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
delay_mks(4600);//60
                       /* Display off, cursor off, blink off */
PORT_LCD = 0x00;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
PORT_LCD = 0x08;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
delay_mks(4600);//60
                       /* Display on */
PORT_LCD = 0x00;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
PORT_LCD = 0x0C;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
delay_mks(240);//60

PORT_LCD = 0x00;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
PORT_LCD = 0x01;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
delay_mks(240);//60

                       /* Set entry mode, cursor moving right */
PORT_LCD = 0x00;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
PORT_LCD = 0x06;    /* �������� ������ � ������� ����������/���������� */
LoadLcd();             /* ��������� ������ � LCD ��������� */
delay_mks(4600);//60
RESET_TWO_LINE;
}

void lcd_int(void)
// ���������� ������ ������������
// ����� ���������� � ����������� �������
{
 unsigned char disp_temp; /* ��������� ������� ������� */
 unsigned char kbd;
// TCNT0 = 0xAF; /* ������������� ������ */

 if(!--sec_recount)
 {
  SET_SECONDS; /* ���������� ���� "��������� �������" */
  sec_recount = 1000;
//  sec_recount = 540;
 }
 if(!--blink_count)
 {
  TOGGLE_BLINK;
  blink_count = 300;
//  if(ALARM) /* ��������� ���� "�������" */
//   BELL_TOGGLE;
 }
 if(START_DELAY) /* ��������� ���� ���������� ������ �������� */
 {
  if(!--repeat_count) /* ���� �������� ������ �������� */
  {
   SET_STOP_DELAY; /* ���������� ���� ��������� �������� */
   RESET_START_DELAY; /* �������� ���� ������ �������� */
  }
 }
 if(START_BEEP) /* ��������� ���� ������ �������� ��������� ������� */
 {
  BELL_TOGGLE;
  if(!--beep_count) /* ���� �������� ������ �������� */
  {
   BELL_OFF; /* ��������� �������� ������ */
   RESET_START_BEEP; /* �������� ���� ������ �������� ��������� ������� */
  }
 }
//KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK
 kbd = (PING & 0x0F) ; // 
// kbd >>= 4;

if(!(kbd & kbd_shift)) /* ������� ��������� ���������� �� �������� ���� ����� PA */
{                       /* ������� ������ ������ */
 if(!(keyboard & kbd_shift)) /* ���� ������ �� ���� �������� */
  {
   if(!SCAN_READY) /* ���� ���������� ����-��� ��������� */
   {
    scan_code = 4-kbd_count; /* ����� ����-��� ������ */
    SET_SCAN_READY; /* ���������� ���� ���������� ����-���� */
   }
   BELL_ON; /* �������� ������ ������� ������ */
   SET_START_BEEP; /* ���������� ���� ������ �������� ��������� ������� */
   beep_count = DELAY_BEEP;  /* ������������ � �� ��������� ������� */
   SET_START_DELAY; /* ���������� ���� ������ �������� ����� ������������ */
   repeat_count = DELAY_REPEAT; /* �������� � �� ����� ������������ */
  }
 else /* ���� ������ ���� �������� */
  {
   if(STOP_DELAY) /* ��������� ��������� �������� ����� ������������ */
   {
    SET_KEYB_REPEAT; /* ���������� ���� �������� �������� ����������� */
    if(!SCAN_READY && AUTO_REPEAT) /* ���� ���������� ����-��� ��������� � �������� ���������� */
    {
     scan_code = 4-kbd_count; /* ����� ����-��� ������ */
     SET_SCAN_READY; /* ���������� ���� ���������� ����-���� */
    }
    RESET_STOP_DELAY; /* �������� ���� ��������� �������� */
    SET_START_DELAY; /* ���������� ���� ������ �������� ������� ����������� */
    repeat_count = TIME_REPEAT; /* ������ ����������� */
   }
  }
 keyboard |= kbd_shift; /* ���������� ��������������� ��� ������� ������ */
}
else /* ������� ������ ������ */
{
 if(keyboard & kbd_shift) /* ���� ������ ���� �������� � �������� */
 {
  RESET_START_DELAY; /* �������� ���� ������ �������� */
  RESET_STOP_DELAY; /* �������� ���� ��������� �������� */
  RESET_KEYB_REPEAT; /* �������� ���� �������� �������� ����������� */
 }
 keyboard &= ~kbd_shift; /* �������� ��������������� ��� ������� ������ */
}

 kbd_count++;
 kbd_shift <<= 1;    /* �������� �� ��������� ������� */

 if(kbd_count == 4) /* ��������� ������� ������ �� ��������� ������ */
 {
  kbd_count = 0;    /* ������� ��������� ����-���� */
  kbd_shift = 0x01; /* ��������� �� ������� ������� ������ */
 }


if(disp_count == 32) /* ��������� ������� �� ��������� ������ */
 {
  disp_count = 0;       /* �������� ������� */
  LCD_CONTROL;          /* ������� ������� ���������� */
  PORT_LCD &= 0xF0;  /* �������� ������� ������� */
  PORT_LCD |= 0x08;  /* ���������� ����� ������� ������� ������ ������ */
  LOAD_LCD;             /* ��������� ������� ������� � ��������� */
  PORT_LCD &= 0xF0;  /* �������� ������� ������� */
  LOAD_LCD;             /* ��������� ������� ������� � ��������� */
  RESET_TWO_LINE;       /* �������� ���� ������ ������ */
  return;
 }
if(disp_count == 16 && !TWO_LINE)
 {
  LCD_CONTROL;          /* ������� ������� ���������� */
  PORT_LCD &= 0xF0;  /* �������� ������� ������� */
  PORT_LCD |= 0x0C;  /* ���������� ����� ������� ������� ������ ������ */
  LOAD_LCD;             /* ��������� ������� ������� � ��������� */
  PORT_LCD &= 0xF0;  /* �������� ������� ������� */
  LOAD_LCD;             /* ��������� ������� ������� � ��������� */
  SET_TWO_LINE;         /* ���������� ���� �������� �� ������ ������ */
  return;
 }
LCD_DATA; /* ������� ������� ������ */
disp_temp = display[disp_count];
if(disp_temp>0x7f)
  disp_temp = TableDecode[display[disp_count] - 0x80];


if(TWO_LINE)   // ���������� ��������� ������
 {             // ������� �� 2 ������
 if(checkbit(blinc_line2,disp_count-16) && BLINK) // ��������� ������ �� �������
  disp_temp = 0x20; // ��� ������� //
 }
else
 {             // ������� �� 1 ������
 if(checkbit(blinc_line1,disp_count) && BLINK) // ��������� ������ �� �������
  disp_temp = 0x20; // ��� �������
 }


PORT_LCD &= 0xF0; /* �������� ������� ������� */
PORT_LCD |= disp_temp >> 4; /* �������� ������� ������� � ������� ����������/���������� */
LOAD_LCD;              /* ��������� ������ � LCD ��������� */
PORT_LCD &= 0xF0; /* �������� ������� ������� */
PORT_LCD |= (disp_temp & 0x0F); /* �������� ������� ������� � ������� ����������/���������� */
LOAD_LCD;              /* ��������� ������ � LCD ��������� */
disp_count ++;
}



// ���������� ���� ��������� ������ ����������
unsigned char scan_ready(void)
{
  return SCAN_READY;
}

// ������ ���� � ���������
unsigned char read_key(void)
{
  while (!SCAN_READY) _NOP();
{
// ShowDigit(30,3,scan_code);
}
RESET_SCAN_READY;
  return scan_code;
}  

void auto_repeat( int zn )
{
    if ( zn==0 )  RESET_AUTO_REPEAT;
    else          SET_AUTO_REPEAT;
}

#include "lcd_user.c"




#ifndef lcd16__h
#define lcd16__h

extern unsigned char lcd_display[32];
void lcd_int(void);
void InitLcd(void);     /* ������������� ���������� */
void ClearDisplay(void); /* �������� ��������� */
void ClearDisplay1(void);
// ���������� ���� ��������� ������ ����������
unsigned char scan_ready(void);
// ������ ���� � ���������
unsigned char read_key(void);

void auto_repeat( int zn );

#include "lcd_user.h"

#endif

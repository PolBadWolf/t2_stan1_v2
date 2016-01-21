#include "main.h"

// -------------------------------------------
// ��������� ���� �� EEPROM
unsigned char ReadEeprom(unsigned char addr)
{
    while (EECR & 0x02);    // ��������� ����������
    _CLI();                 // ��������� ����������
    EEARH = 0;              // ������������ ������� 256 ���� EEPROM
    EEARL = addr;
    EECR |= 0x01;           // ���������� ��� ���������� ������
    _SEI();                 // ��������� ����������
    return EEDR;            // ��������� ����
}

// �������� ���� � EEPROM
void WriteEeprom(unsigned char addr,  unsigned char data)
{
    EEARH = 0;          // ������������ ������� 256 ���� EEPROM
    while(EECR & 0x02); // ��������� ����������
    EEARL = addr;       // �������� ���� � ������� ������ EEPROM
    EEDR = data;        // �������� ���� � ������� ������ EEPROM
    _CLI();             // ��������� ����������
    EECR |= 0x04;       // ���������� ��� ���������� ������
    EECR |= 0x02;       // ���������� ��� ������
    _SEI();             // ��������� ����������
}

// PrnEepromString( Screen, addr, len );
/*void PrnEepromString( unsigned char pos, unsigned char addr, unsigned char len )
{
 unsigned char *s;
 unsigned char flag=0;
 unsigned char dat;
 unsigned char ofset=0;
 s = display + pos;

 while(flag==0)
 {
 dat = ReadEeprom( addr + ofset );
 if ( dat==0 ) flag = 1;
 else
  {
    *s++ = dat;
    ofset++;
    if ( (ofset >= len) && ( len != 0 ) ) flag = 1;
  }
 }
}
*/

// InpEepromString( addr, len, "Stroka" );
// InpEepromString( addr, len, Variable );
void WriteEepromString( unsigned char addr, unsigned char len, unsigned char *string )
{
    unsigned char flag=0;
    unsigned char ofset=0;
    unsigned char dat;
    while(flag==0)
    {
        dat = *string++;
        WriteEeprom( addr+ofset , dat );
        if ( dat == 0 ) 
            flag=1;
        else
        {
            if ( (++ofset >= len) )
            {
                flag=1;
                WriteEeprom( addr+ofset , 0 );
            }
        }
    }
}

// OutEepromString( addr, len, Variable );
void ReadEepromString( unsigned char addr, unsigned char len, unsigned char *string )
{
    unsigned char flag=0;
    unsigned char ofset=0;
    unsigned char dat;
    while(flag==0)
    {
        dat = ReadEeprom(addr+ofset);
        *string++ = dat;
        if ( dat == 0 ) flag=1;
        if ( ++ofset >= len ) flag=1;
    }
}

//    dat = *string++;
//    WriteEeprom( addr+ofset , dat );

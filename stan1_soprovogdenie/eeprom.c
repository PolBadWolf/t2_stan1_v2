#include "main.h"

// -------------------------------------------
// Прочитать байт из EEPROM
unsigned char ReadEeprom(unsigned char addr)
{
    while (EECR & 0x02);    // Дождаться готовности
    _CLI();                 // Запретить прерывания
    EEARH = 0;              // Используются младшие 256 байт EEPROM
    EEARL = addr;
    EECR |= 0x01;           // Установить бит разрешения чтения
    _SEI();                 // Разрешить прерывания
    return EEDR;            // Прочитать байт
}

// Записать байт в EEPROM
void WriteEeprom(unsigned char addr,  unsigned char data)
{
    EEARH = 0;          // Используются младшие 256 байт EEPROM
    while(EECR & 0x02); // Дождаться готовности
    EEARL = addr;       // Записать байт в регистр адреса EEPROM
    EEDR = data;        // Записать байт в регистр данных EEPROM
    _CLI();             // Запретить прерывания
    EECR |= 0x04;       // Установить бит разрешения записи
    EECR |= 0x02;       // Установить бит записи
    _SEI();             // Разрешить прерывания
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

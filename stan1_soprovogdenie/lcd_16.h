
#ifndef lcd16__h
#define lcd16__h

extern unsigned char lcd_display[32];
void lcd_int(void);
void InitLcd(void);     /* Инициализация индикатора */
void ClearDisplay(void); /* Очистить индикатор */
void ClearDisplay1(void);
// возвращает флаг состояния опроса клавиатуры
unsigned char scan_ready(void);
// чтение кода с ожиданием
unsigned char read_key(void);

void auto_repeat( int zn );

#include "lcd_user.h"

#endif

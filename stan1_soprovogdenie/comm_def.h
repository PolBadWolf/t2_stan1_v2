
#ifndef commdef__h
#define commdef__h

/****************************************************************************/
/* Usefull bit macros.                                                      */
/****************************************************************************/
#define checkbit(var,bit)  (var & (0x01 << (bit)))
#define setbit(var,bit)    (var |= (0x01 << (bit)))
#define clrbit(var,bit)    (var &= (~(0x01 << (bit))))
#define invbit(var,bit)    (var ^= (0x01 << (bit)))
#define LOW(x) ((char*)&x)[0]
#define HIGH(x) ((char*)&x)[1]
#define BYTES(x) ((unsigned char *)&(x))
#define WORDS(x) ((unsigned int *)&(x))


typedef unsigned char uchar;
typedef signed char   schar;
typedef unsigned int   uint;
typedef unsigned long ulong;

/*
// Команды для DS1820
#define SKIP_ROM           0xCC
#define MATCH_ROM          0x55
#define READ_ROM           0x33
#define CONVERT_T          0x44
#define READ_SCRATCHPAD    0xBE
#define WRITE_SCRATCHPAD   0x4E
*/

/*
// Область SCRATCHPAD
#define TEMPERATURE_LO     0
#define TEMPERATURE_HI     1
#define USER_BYTE_TH       2
#define USER_BYTE_TL       3
#define RESERVED_1         4
#define RESERVED_2         5
#define COUNT_REMAIN       6
#define COUNT_PER_C        7
#define BYTE_CRC           8
*/

/*
// Регистр флагов edit
#define F_HOUR             0x01 // флаг установки часов
#define F_MIN              0x02 // флаг установки минут
#define F_SETUP            0x04 // флаг setup
*/

/*
#define SET_F_HOUR         reg_edit |= F_HOUR   // Установить флаг установки часов
#define RESET_F_HOUR       reg_edit &= ~F_HOUR  // Сбросить флаг установки часов
#define EDIT_HOUR         (reg_edit & F_HOUR)   // Проверить флаг установки часов
#define SET_F_MIN          reg_edit |= F_MIN    // Установить флаг установки минут
#define RESET_F_MIN        reg_edit &= ~F_MIN   // Сбросить флаг установки минут
#define EDIT_MIN          (reg_edit & F_MIN)    // Проверить флаг установки минут
#define SET_F_SETUP        reg_edit |= F_SETUP  // Установить флаг setup
#define RESET_F_SETUP      reg_edit &= ~F_SETUP // Сбросить флаг setup
#define SETUP             (reg_edit & F_SETUP)  // Проверить флаг setup
*/

/*
//  flags       Регистр флагов
#define F_D1            0x01
#define F_D2            0x02
#define F_D             0x04 
*/

/*
#define SET_D1_BAD         heat_flags |= F_D1   // Установить флаг ДАТЧИК 1 - НЕИСПРАВЕН
#define RESET_D1_BAD       heat_flags &= ~F_D1  // Сбросить флаг ДАТЧИК 1 - НЕИСПРАВЕН
#define D1_BAD            (heat_flags & F_D1)   // Проверить флаг ДАТЧИК 1 - НЕИСПРАВЕН

#define SET_D2_BAD         heat_flags |= F_D2   // Установить флаг ДАТЧИК 2 - НЕИСПРАВЕН
#define RESET_D2_BAD       heat_flags &= ~F_D2  // Сбросить флаг ДАТЧИК 2 - НЕИСПРАВЕН
#define D2_BAD            (heat_flags & F_D2)   // Проверить флаг ДАТЧИК 2 - НЕИСПРАВЕН

#define SET_D_BAD         heat_flags |= F_D     // Установить флаг ДАТЧИКИ - НЕИСПРАВНЫ
#define RESET_D_BAD       heat_flags &= ~F_D    // Сбросить флаг ДАТЧИКИ - НЕИСПРАВНЫ
#define D_BAD            (heat_flags & F_D)     // Проверить флаг ДАТЧИКИ - НЕИСПРАВНЫ
*/

//#define SET_         manual_flags |= F_
//#define RESET_       manual_flags &= ~F_
//#define              (manual_flags & F_) /* Проверка флага "" */

/*
// Символы для основной работы
#define SYM_UP_LINE     0x05 // Символ "Верхняя черта"
#define SYM_GRADUS      0x07 // Символ "Градус цельсия"
#define SYM_RX          0x04 // Символ "RX"
#define SYM_UP          0x80 // Символ "Кондиционер: нагрев"
#define SYM_DOWN        0x81 // Символ "Кондиционер: охлаждение"
*/

/*
#define DELAY_REPEAT       700 // Задержка в мс перед автоповтором
#define TIME_REPEAT        400 // Период автоповтора
#define DELAY_BEEP          60 // Длительности в мс звукового сигнала
*/

/*
#define MIN_OVER             1    // Признак перехода через минимальное значение
#define MAX_OVER             1    // Признак перехода через максимальное значение
#define MIN_END              0    // Признак остановки на минимальном значении
#define MAX_END              0    // Признак остановки на максимальном значении

#define MIN_PERIOD           5    // Минимальный период управления в сек
#define DEFAULT_PERIOD       10   // Период управления в сек по умолчанию
#define MAX_PERIOD           20   // Максимальный период управления в сек

#define TIMEOUT_RETURN       5    // Время таймаута возвращения в рабочий режим
#define LEN_PASSWORD         5    // Длина пароля 5 скан-кодов
#define CONST_AVERAG         16

#define MAX_UST_HOUR          23
#define DEFAULT_UST_HOUR       1

#define MAX_UST_MINUTE        59
#define DEFAULT_UST_MINUTE     0


#define ADR_BASE_ROM           0 // Начальный адрес 6x8 сериальных номеров DS1820
*/

#endif


#ifndef main__h
#define main__h
#include <ioavr.h>
#include <ina90.h>
#include <inavr.h>

// частота чипа
#define  C_Fosc  16000000
#define RS232
// #define RS485
// ==============================
#define _delay_us(us)     __delay_cycles((C_Fosc / 1000000) * (us));
#define _delay_ms(ms)     __delay_cycles((C_Fosc / 1000) * (ms));

#include "lcd_16.h"
#include "timer.h"
#include "indicator.h"
#include "defect.h"
#include "rez.h"
#include "upor.h"
#include "pulsar.h"
#include "eeprom_user.h"
#include "koleso.h"
#include "rs232.h"
#include "rs485.h"
#include "OutDataPc.h"
#include "crc.h"

#ifdef RS232
#define RSPC
#define rspc_init       rs232_init
#define rspc_WriteByte  rs232_WriteByte
#define rspc_ReadByte   rs232_ReadByte
#define rspc_RxLen      rs232_RxLen
#define rspc_RxOverFlow rs232_RxOverFlow
#define rspc_TxLen      rs232_TxLen
#define rspc_TxOverFlow rs232_TxOverFlow
#endif

#ifdef RS485
#define RSPC
#define rspc_init       rs485_init
#define rspc_WriteByte  rs485_WriteByte
#define rspc_ReadByte   rs485_ReadByte
#define rspc_RxLen      rs485_RxLen
#define rspc_RxOverFlow rs485_RxOverFlow
#define rspc_TxLen      rs485_TxLen
#define rspc_TxOverFlow rs485_TxOverFlow
#endif



#endif

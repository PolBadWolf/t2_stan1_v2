
#ifndef macros__h
#define macros__h

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
#define xByte(x,b) ((char*)&x)[b]


typedef unsigned char uchar;
typedef signed char   schar;
typedef unsigned int   uint;
typedef unsigned long ulong;

#endif

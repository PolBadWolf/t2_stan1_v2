
#ifdef RS232

#ifndef rs232__h
#define rs232__h
  void rs232_init();
  unsigned char rs232_WriteByte(unsigned char data);
  unsigned char rs232_ReadByte(unsigned char *Byte);
  extern unsigned char rs232_RxLen;       // заполнение буффера
  extern unsigned char rs232_RxOverFlow;  // переполнение при приеме
  extern unsigned char rs232_TxLen;       // заполнение буффера
  extern unsigned char rs232_TxOverFlow;  // переполнение при передаче
#endif

#endif

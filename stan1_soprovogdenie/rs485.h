
#ifdef RS485

#ifndef rs485__h
#define rs485__h
  void rs485_init();
  unsigned char rs485_WriteByte(unsigned char data);
  unsigned char rs485_ReadByte(unsigned char *Byte);
  extern unsigned char rs485_RxLen;       // ���������� �������
  extern unsigned char rs485_RxOverFlow;  // ������������ ��� ������
  extern unsigned char rs485_TxLen;       // ���������� �������
  extern unsigned char rs485_TxOverFlow;  // ������������ ��� ��������
#endif

#endif

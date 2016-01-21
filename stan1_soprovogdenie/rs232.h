
#ifdef RS232

#ifndef rs232__h
#define rs232__h
  void rs232_init();
  unsigned char rs232_WriteByte(unsigned char data);
  unsigned char rs232_ReadByte(unsigned char *Byte);
  extern unsigned char rs232_RxLen;       // ���������� �������
  extern unsigned char rs232_RxOverFlow;  // ������������ ��� ������
  extern unsigned char rs232_TxLen;       // ���������� �������
  extern unsigned char rs232_TxOverFlow;  // ������������ ��� ��������
#endif

#endif


#include "main.h"

#ifdef RS232
//====================================
#include "rs232.h"
//===
//------------------------------------
// скорость
#define rs_baud 38400
//------------------------------------
// pariry : disable, even, odd
#define rs_parity_disable
//------------------------------------
// количество бит 5, 6, 7, 8
#define rs_bits_8
//------------------------------------
// UART Buffer Defines
// 1,2,4,8,16,32,64,128 or 256 bytes
#define UART_RX232_BUFFER_SIZE 32
#define UART_RX232_BUFFER_MASK ( UART_RX232_BUFFER_SIZE - 1 )
// 1,2,4,8,16,32,64,128 or 256 bytes
#define UART_TX232_BUFFER_SIZE 32
#define UART_TX232_BUFFER_MASK ( UART_TX232_BUFFER_SIZE - 1 )
#if (UART_RX232_BUFFER_SIZE & UART_RX232_BUFFER_MASK)
#error RS232 RX buffer size is not a power of 2
#endif
#if (UART_TX232_BUFFER_SIZE & UART_TX232_BUFFER_MASK)
#error RS232 TX buffer size is not a power of 2
#endif

//namespace ns_rs232
//{
  unsigned char rs232_RxBuf[UART_RX232_BUFFER_SIZE];
  unsigned char rs232_RxHead;
  unsigned char rs232_RxTail;
  unsigned char rs232_TxBuf[UART_TX232_BUFFER_SIZE];
  unsigned char rs232_TxHead;
  unsigned char rs232_TxTail;
  unsigned char rs232_RxLen;       // заполнение буффера
  unsigned char rs232_RxOverFlow;  // переполнение при приеме
  unsigned char rs232_TxLen;       // заполнение буффера
  unsigned char rs232_TxOverFlow;  // переполнение при передаче
  //----------
  void rs232_init()
  {
    unsigned int baud;
    baud = (unsigned int)( (unsigned long)C_Fosc/((unsigned long)rs_baud*16) );
    UBRR0H = (unsigned char)(baud>>8);
    UBRR0L = (unsigned char)baud;
    // Parity
    #define parity_error
    UCSR0C = 0;
    #ifdef rs_parity_disable
    UCSR0C_UPM00 = 0;
    UCSR0C_UPM01 = 0;
    #undef parity_error
    #endif
    #ifdef rs_parity_even
    UCSR0C_UPM00 = 0;
    UCSR0C_UPM01 = 1;
    #undef parity_error
    #endif
    #ifdef rs_parity_odd
    UCSR0C_UPM00 = 1;
    UCSR0C_UPM01 = 1;
    #undef parity_error
    #endif
    #ifdef parity_error
    #error RS232 Error define parity : only disable, even or odd
    #endif
    // Bits
    #define bits_error
    #ifdef rs_bits_5
    UCSR0C_UCSZ00 = 0;
    UCSR0C_UCSZ01 = 0;
    UCSR0B_UCSZ02 = 0;
    #undef  bits_error
    #endif
    #ifdef rs_bits_6
    UCSR0C_UCSZ00 = 1;
    UCSR0C_UCSZ01 = 0;
    UCSR0B_UCSZ02 = 0;
    #undef  bits_error
    #endif
    #ifdef rs_bits_7
    UCSR0C_UCSZ00 = 0;
    UCSR0C_UCSZ01 = 1;
    UCSR0B_UCSZ02 = 0;
    #undef  bits_error
    #endif
    #ifdef rs_bits_8
    UCSR0C_UCSZ00 = 1;
    UCSR0C_UCSZ01 = 1;
    UCSR0B_UCSZ02 = 0;
    #undef  bits_error
    #endif
    #ifdef bits_error
    #error RS232 error define bits, only 5, 6, 7, 8
    #endif
    // 1 стоп бит
    UCSR0C_USBS0 = 0;
    // начальные значения
    rs232_RxHead =  rs232_RxTail = rs232_TxHead = rs232_TxTail = rs232_RxLen = rs232_RxOverFlow = rs232_TxLen = rs232_TxOverFlow = 0;
    //enable UART receiver and transmitter,transmitte and receive interrupt
    UCSR0B = 0;
    UCSR0B_RXCIE0 = 1;
    UCSR0B_RXEN0  = 1;
    UCSR0B_TXEN0  = 1;
    UCSR0B_TXCIE0 = 1;
  }
  //===================================
  #pragma vector = USART0_RXC_vect // Прерывание приема от RS232
  __interrupt void UART232_RX_interrupt(void)
  {
    unsigned char data;
    unsigned char tmphead;
    // read the received data
    #ifdef rs_bits_5
    data = UDR0 & 0x1f;
    #endif
    #ifdef rs_bits_6
    data = UDR0 & 0x3f;
    #endif
    #ifdef rs_bits_7
    data = UDR0 & 0x7f;
    #endif
    #ifdef rs_bits_8
    data = UDR0;
    #endif
    // calculate buffer index
    tmphead = (rs232_RxHead + 1) & UART_RX232_BUFFER_MASK;
    if(tmphead == rs232_RxTail)
    {
      // ERROR! Receive buffer overflow
      rs232_RxOverFlow = 1;
    }
    else
    {
      rs232_RxOverFlow = 0;
      rs232_RxHead = tmphead; // store new index
      rs232_RxBuf[rs232_RxHead] = data; // store received data in buffer
      rs232_RxLen++;
    }
  }
  //===================================
  #pragma vector = USART0_UDRE_vect /* Прерывание передачи от RS232 */
  __interrupt void UART_TX_interrupt(void)
  {
    unsigned char tmptail;
    // check if all data is transmitted
    if(rs232_TxHead != rs232_TxTail)
    {
      // calculate buffer index
      tmptail = (rs232_TxTail + 1) & UART_TX232_BUFFER_MASK;
      rs232_TxTail = tmptail;       // store new index
      UCSR0B_TXB80 = 1;
      UDR0 = rs232_TxBuf[tmptail];  // start transmition
      rs232_TxLen--;
    }
    else
    {
      UCSR0B_UDRIE0 = 0;   // disable UDRE interrupt
      //
      rs232_TxLen = 0;
    }
  }
  //===================================
  unsigned char rs232_ReadByte(unsigned char *Byte)
  {
    unsigned char tmptail;
    unsigned char stat;
    unsigned char cs;
    cs = __save_interrupt();
    __disable_interrupt();
    if (rs232_RxHead==rs232_RxTail)
    {
      rs232_RxLen = 0;
      stat = 0; // wait for incomming data
    }
    else
    {
      // calculate buffer index
      tmptail = (rs232_RxTail + 1) & UART_RX232_BUFFER_MASK;
      rs232_RxTail = tmptail;          // store new index
      *Byte = rs232_RxBuf[tmptail];    // return data
      rs232_RxLen--;
      stat = 1;
    }
    __restore_interrupt(cs);
    return stat;
  }
  //===================================
  unsigned char rs232_WriteByte(unsigned char data)
  {
    unsigned char tmphead;
    unsigned char stat;
    unsigned char cs;
    cs = __save_interrupt();
    __disable_interrupt();
    tmphead = (rs232_TxHead + 1) & UART_TX232_BUFFER_MASK; 
    if (tmphead==rs232_TxTail)
    {
      rs232_TxOverFlow = 1;
      stat = 0;// There is no free place in the buffer
    }
    else
    {      
      rs232_TxBuf[tmphead] = data; // store data in buffer
      rs232_TxHead = tmphead; // store new index
      UCSR0B_UDRIE0 = 1; // enable UDRE interrupt
      rs232_TxLen++;
      stat = 1;
    }
    __restore_interrupt(cs);
    return stat;
//  }
  //===================================
}
#undef rs_baud
#undef rs_parity
#undef rs_bits
#undef UART_RX_BUFFER_SIZE
#undef UART_RX_BUFFER_MASK
#undef UART_TX_BUFFER_SIZE
#undef UART_TX_BUFFER_MASK
//====================================
#endif


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
// ������� ��� DS1820
#define SKIP_ROM           0xCC
#define MATCH_ROM          0x55
#define READ_ROM           0x33
#define CONVERT_T          0x44
#define READ_SCRATCHPAD    0xBE
#define WRITE_SCRATCHPAD   0x4E
*/

/*
// ������� SCRATCHPAD
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
// ������� ������ edit
#define F_HOUR             0x01 // ���� ��������� �����
#define F_MIN              0x02 // ���� ��������� �����
#define F_SETUP            0x04 // ���� setup
*/

/*
#define SET_F_HOUR         reg_edit |= F_HOUR   // ���������� ���� ��������� �����
#define RESET_F_HOUR       reg_edit &= ~F_HOUR  // �������� ���� ��������� �����
#define EDIT_HOUR         (reg_edit & F_HOUR)   // ��������� ���� ��������� �����
#define SET_F_MIN          reg_edit |= F_MIN    // ���������� ���� ��������� �����
#define RESET_F_MIN        reg_edit &= ~F_MIN   // �������� ���� ��������� �����
#define EDIT_MIN          (reg_edit & F_MIN)    // ��������� ���� ��������� �����
#define SET_F_SETUP        reg_edit |= F_SETUP  // ���������� ���� setup
#define RESET_F_SETUP      reg_edit &= ~F_SETUP // �������� ���� setup
#define SETUP             (reg_edit & F_SETUP)  // ��������� ���� setup
*/

/*
//  flags       ������� ������
#define F_D1            0x01
#define F_D2            0x02
#define F_D             0x04 
*/

/*
#define SET_D1_BAD         heat_flags |= F_D1   // ���������� ���� ������ 1 - ����������
#define RESET_D1_BAD       heat_flags &= ~F_D1  // �������� ���� ������ 1 - ����������
#define D1_BAD            (heat_flags & F_D1)   // ��������� ���� ������ 1 - ����������

#define SET_D2_BAD         heat_flags |= F_D2   // ���������� ���� ������ 2 - ����������
#define RESET_D2_BAD       heat_flags &= ~F_D2  // �������� ���� ������ 2 - ����������
#define D2_BAD            (heat_flags & F_D2)   // ��������� ���� ������ 2 - ����������

#define SET_D_BAD         heat_flags |= F_D     // ���������� ���� ������� - ����������
#define RESET_D_BAD       heat_flags &= ~F_D    // �������� ���� ������� - ����������
#define D_BAD            (heat_flags & F_D)     // ��������� ���� ������� - ����������
*/

//#define SET_         manual_flags |= F_
//#define RESET_       manual_flags &= ~F_
//#define              (manual_flags & F_) /* �������� ����� "" */

/*
// ������� ��� �������� ������
#define SYM_UP_LINE     0x05 // ������ "������� �����"
#define SYM_GRADUS      0x07 // ������ "������ �������"
#define SYM_RX          0x04 // ������ "RX"
#define SYM_UP          0x80 // ������ "�����������: ������"
#define SYM_DOWN        0x81 // ������ "�����������: ����������"
*/

/*
#define DELAY_REPEAT       700 // �������� � �� ����� ������������
#define TIME_REPEAT        400 // ������ �����������
#define DELAY_BEEP          60 // ������������ � �� ��������� �������
*/

/*
#define MIN_OVER             1    // ������� �������� ����� ����������� ��������
#define MAX_OVER             1    // ������� �������� ����� ������������ ��������
#define MIN_END              0    // ������� ��������� �� ����������� ��������
#define MAX_END              0    // ������� ��������� �� ������������ ��������

#define MIN_PERIOD           5    // ����������� ������ ���������� � ���
#define DEFAULT_PERIOD       10   // ������ ���������� � ��� �� ���������
#define MAX_PERIOD           20   // ������������ ������ ���������� � ���

#define TIMEOUT_RETURN       5    // ����� �������� ����������� � ������� �����
#define LEN_PASSWORD         5    // ����� ������ 5 ����-�����
#define CONST_AVERAG         16

#define MAX_UST_HOUR          23
#define DEFAULT_UST_HOUR       1

#define MAX_UST_MINUTE        59
#define DEFAULT_UST_MINUTE     0


#define ADR_BASE_ROM           0 // ��������� ����� 6x8 ���������� ������� DS1820
*/

#endif

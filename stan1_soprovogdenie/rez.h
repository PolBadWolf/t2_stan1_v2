
#ifndef rez__h
#define rez__h

// ���� �������� ������� ����
extern unsigned char rez_fg;
extern unsigned char rez_fg1;
// ������ ��������� ������� ����
extern unsigned char rez_tr;

// ������ :
// ������ ��������
extern unsigned char rez_ms_def[];
// ������ ������ �����
extern unsigned int rez_ms_ind[];
// � ����� ����� �� ����� ���� �� �������
extern unsigned int rez_end;

// ������ �������
unsigned char rez_test(void);

void rez_int(void);
// ��������� - �����
void rez_init(void);

#endif


// ������� ������ �� ��������� � �������� �������
//void Show(unsigned char pos, unsigned char flash* t);

// ������� ������ �� ��������� � �������� �������
//void ShowChar(unsigned char pos, unsigned char ch);

// �������� �������� ����������� � ���������� ���� 0.00 � ���������� �������
//void ShowTemperature(unsigned char pos, unsigned int digit);

// �������� ����� � �������� ��������� � ���������� ���� � ���������� ������� � ������������ ����������� ����
///void ShowDigit(unsigned char pos, unsigned char numdigit, unsigned int digit);

// �������� ����� � �������� ������ � ���������� ���� � ���������� ������� � ������������ ����������� ����
void ShowDigitZ(unsigned char pos, unsigned char numdigit, unsigned long digit);

//void Delay4mks(void);

void Show_m2(unsigned char pos, unsigned char *string);

/* ��������� �� ���������� ������, ������ �������, ����������� ��������, ������������ ��������, ������� �������� ����� ����������� �������� */
void ParametrDown(void *ptr,unsigned int size,unsigned int min_param,unsigned int max_param,unsigned char over_min);

/* ��������� �� ���������� ������, ������ �������, ����������� ��������, ������������ ��������, ������� �������� ����� ������������ �������� */
void ParametrUp(void *ptr,unsigned int size,unsigned int min_param,unsigned int max_param,unsigned char over_max);


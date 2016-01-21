
// Вывести строку на индикатор с заданной позиции
//void Show(unsigned char pos, unsigned char flash* t);

// Вывести символ на индикатор с заданной позиции
//void ShowChar(unsigned char pos, unsigned char ch);

// Показать значение температуры в десятичном виде 0.00 в конкретной позиции
//void ShowTemperature(unsigned char pos, unsigned int digit);

// Показать число с ведущими пробелами в десятичном виде в конкретной позиции с определенным количеством цифр
///void ShowDigit(unsigned char pos, unsigned char numdigit, unsigned int digit);

// Показать число с ведущими нулями в десятичном виде в конкретной позиции с определенным количеством цифр
void ShowDigitZ(unsigned char pos, unsigned char numdigit, unsigned long digit);

//void Delay4mks(void);

void Show_m2(unsigned char pos, unsigned char *string);

/* Указатель на изменяемый объект, размер объекта, минимальное значение, максимальное значение, признак перехода через минимальное значение */
void ParametrDown(void *ptr,unsigned int size,unsigned int min_param,unsigned int max_param,unsigned char over_min);

/* Указатель на изменяемый объект, размер объекта, минимальное значение, максимальное значение, признак перехода через максимальное значение */
void ParametrUp(void *ptr,unsigned int size,unsigned int min_param,unsigned int max_param,unsigned char over_max);


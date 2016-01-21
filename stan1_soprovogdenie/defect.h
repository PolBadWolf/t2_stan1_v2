
#ifndef defect__h
#define defect__h

// флаг состояния дефектов
extern unsigned char defect_fg_1;
extern unsigned char defect_fg_2;
extern unsigned char defect_fg_3;
extern unsigned char defect_fg_4;
extern unsigned char defect_fg_5;
extern unsigned char defect_fg_6;
extern unsigned char defect_fg_7;
extern unsigned char defect_fg_8;

// массив дефектов до трубореза
extern unsigned char d_def[];
extern unsigned int d_def_x;
// адреса растояния до труборезки от дефектоскопов
extern unsigned int d_def_sm[];
extern unsigned int d_rez_sm;
// массив дефектов от краско отметчиков до линии реза
extern unsigned char s_def[];

// индикация дефекта от краскоотметчика до реза
extern unsigned char ind_d_korez;

void defect_int(void);
// настройка переменных
void defect_init(void);

//----------------------------------------
// чтение датчика
unsigned char defect_test_1(void);
unsigned char defect_test_2(void);
unsigned char defect_test_3(void);
unsigned char defect_test_4(void);
unsigned char defect_test_5(void);
unsigned char defect_test_6(void);
unsigned char defect_test_7(void);
unsigned char defect_test_8(void);

unsigned int indx_norm( signed int data_in);

void indx_inc(void);

// тригер состояние датчиков дефектов
extern unsigned char defect_tr_1;
extern unsigned char defect_tr_2;
extern unsigned char defect_tr_3;
extern unsigned char defect_tr_4;
extern unsigned char defect_tr_5;
extern unsigned char defect_tr_6;
extern unsigned char defect_tr_7;
extern unsigned char defect_tr_8;


#endif

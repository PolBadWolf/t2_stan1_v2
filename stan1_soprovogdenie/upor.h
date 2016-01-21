
#ifndef upor__h
#define upor__h

// регистры на датчиках для сопровождения дефектов трубы 
extern unsigned char defect1;
extern unsigned char defect2;
extern unsigned char defect3;
extern unsigned char defect4;
extern unsigned char defect5;
extern unsigned char defect1r;
extern unsigned char defect2r;
extern unsigned char defect3r;
extern unsigned char defect4r;


// флаг сработки датчика наличия трубы
extern unsigned char trub_fg_1;
extern unsigned char trub_fg_2;
extern unsigned char trub_fg_3;
extern unsigned char trub_fg_4;
extern unsigned char trub_fg_5;
extern unsigned char trub_fg_1i;
extern unsigned char trub_fg_2i;
extern unsigned char trub_fg_3i;
extern unsigned char trub_fg_4i;
extern unsigned char trub_fg_5i;

extern unsigned char trub_tr_1;
extern unsigned char trub_tr_2;
extern unsigned char trub_tr_3;
extern unsigned char trub_tr_4;
extern unsigned char trub_tr_5;

// чтение датчика
unsigned char trub_test_1(void);
unsigned char trub_test_2(void);
unsigned char trub_test_3(void);
unsigned char trub_test_4(void);
unsigned char trub_test_5(void);

void upor_init(void);
void upor_int(void);

// упор вверх
void upor_up(void);

// упор вниз
void upor_dn(void);

#endif

#include "main.h"

// часы для отладки
extern unsigned int t_msec;
extern unsigned char t_sec;
extern unsigned char t_min;
extern unsigned char t_chas;

// индикация дефектов
extern unsigned char ind_def8[8];
extern unsigned char ind_def12[12];
extern unsigned char def_rez;
extern unsigned char def_rez_out;

// номер экрана выводимый на дисплей
unsigned int n_screen;

// индикация труб после трубо отрезного станка
extern unsigned char dt_12;
extern unsigned char dt_23;
extern unsigned char dt_34;
extern unsigned char dt_45;

// длина трубы в папугаях
extern unsigned int tube_len;

// флаг индикации состояния упора
extern unsigned char upor_flag;

unsigned char old_sec;
unsigned char xpen=0;
// time out
unsigned int timeout_key=0;

unsigned char cur_key;

//    пароль
// номер символа для ввола пароля
unsigned char psw_n;
// для сбора пароля
unsigned long psw_summa;
// переменная ввода цифры
unsigned char psw_temp;

//-------------------
// коррекция
unsigned char dev_n;
// временная переменная
unsigned int dev_l;

extern unsigned int tube_count;
extern unsigned int tube_count_def;

extern unsigned char ind_def_k;


//----------------------------------------
// 12.08.2008
// работа с 3 краскопультом ( для тощиномера )
//
// переменнные в main.c
// время - длительность импульса покраски
// для переднего конца трубы
extern int kr3_dl_const;
// время задержки  от переднего конца трубы ( м сек ) до заднего
extern int kr3_zd_const;
// переменная для редактирования
int kr3_temp;
extern unsigned char ind_def_k;

void indicator_init(void)
{
    n_screen=3;
    xpen=0;
}

void indicator_cicle(void)
{
    //-------------------------------------------
    // индикатор работа
    if ( old_sec != t_sec )
    {
        unsigned char bit1;
        unsigned char bit2;
        old_sec = t_sec;
        DDRE |= 1<<3;
        bit1 = PORTE;
        bit2 = bit1 & ~(1<<3);
        PORTE = bit2 | ( (~bit1) & (1<<3) );
    }
    // кнопки
    if ( scan_ready()!=0 )
        cur_key=read_key();
    else
        cur_key=255;

//  ShowDigitZ(15,1,t_sec);
//  ShowDigitZ(31,1,n_screen);
    xpen=0;
    if ( n_screen==0)
    {
        Show_m2(0,"ошибка индикации");
    }
    if ( n_screen==1 && xpen==0 )
    {
        xpen=1;
        //==============================================
        // блокировка индикации
        ClearDisplay1();
        n_screen=3;
        //=========================================

        // индикация с порта
        Show_m2(0,"дефекты1");
        ShowDigitZ(16,1,defect_test_1());
        ShowDigitZ(17,1,defect_test_2());
        ShowDigitZ(18,1,defect_test_3());
        ShowDigitZ(19,1,defect_test_4());
        ShowDigitZ(20,1,defect_test_5());
        ShowDigitZ(21,1,defect_test_6());
        ShowDigitZ(22,1,defect_test_7());
        ShowDigitZ(23,1,defect_test_8());
        Show_m2(9,"К");
        ShowDigitZ(25,1,koleso_test());
        Show_m2(10,"Р");
        ShowDigitZ(26,1,rez_test());
        //-----
        if ( cur_key==1 )
        {
            ClearDisplay1();
            n_screen=2;
        }
    }
    if ( n_screen==2 && xpen==0 )
    {
        xpen=1;
        // индикация с порта
        Show_m2(0,"дефекты2");
        ShowDigitZ(16,1,defect_fg_1);
        ShowDigitZ(17,1,defect_fg_2);
        ShowDigitZ(18,1,defect_fg_3);
        ShowDigitZ(19,1,defect_fg_4);
        ShowDigitZ(20,1,defect_fg_5);
        ShowDigitZ(21,1,defect_fg_6);
        ShowDigitZ(22,1,defect_fg_7);
        ShowDigitZ(23,1,defect_fg_8);
        Show_m2(9,"К");
        ShowDigitZ(25,1,d_koleso_tr);
        Show_m2(10,"Р");
        ShowDigitZ(26,1,rez_tr);
        //-----
        if ( cur_key==1 )
        {
            ClearDisplay1();
            n_screen=3;
        }
    }

    if ( n_screen==3 && xpen==0 )
    {
        xpen=1;
        // индикация сопровождения дефекта
        Show_m2(0,"сопровождение");
        {
            unsigned char j_ind;
            unsigned char i_ind;
            j_ind=11;

            for (i_ind=0;i_ind<12;)
            {
                if ( ind_def12[j_ind] ==0 )
                    Show_m2(16+i_ind,"_");
                else
                    Show_m2(16+i_ind,"i");
                j_ind--;
                i_ind++;
            }
        }
        // колесо
        if ( d_koleso_tr==0 )
            Show_m2(28," ");
        else
            Show_m2(28,"К");
        // рез
        if ( rez_tr==0 )
            Show_m2(29,"_");
        else
            Show_m2(29,"Р");
        // собраный дефект перед резом
        if ( ind_d_korez==0 )
            Show_m2(30,"x");
        else
            Show_m2(30,"D");
        //-----
        if ( cur_key==1 )
        {
            ClearDisplay1();
            n_screen=4;
        }
        if ( cur_key==3 )
        {
            d_def[ indx_norm(d_def_sm[1]) ] |= 1<<1;
            ind_def8[1]=1;
        }
    }

    if ( n_screen==4 && xpen==0 )
    {
        // на станке отображение
        xpen=1;
        // колесо
        if ( d_koleso_tr==0 )
            Show_m2(0,"_");
        else
            Show_m2(0,"К");
        // рез
        if ( rez_tr==0 )
            Show_m2(1,"_");
        else
            Show_m2(1,"Р");
        // собраный дефект перед резом
        if ( ind_d_korez==0 )
            Show_m2(16,"x");
        else
            Show_m2(16,"D");
        // сопровождение трубы на станке
        // 1
        ShowDigitZ(3,3,rez_ms_ind[1]);
        if ( rez_ms_def[1]==0 )
            Show_m2(6," ");
        else
            Show_m2(6,"D");
        // 2
        ShowDigitZ(8,3,rez_ms_ind[2]);
        if ( rez_ms_def[2]==0 )
            Show_m2(11," ");
        else
            Show_m2(11,"D");
        // 3
        ShowDigitZ(19,3,rez_ms_ind[3]);
        if ( rez_ms_def[3]==0 )
            Show_m2(22," ");
        else
            Show_m2(22,"D");
        // 4
        ShowDigitZ(24,3,rez_ms_ind[4]);
        if ( rez_ms_def[4]==0 )
            Show_m2(27," ");
        else
            Show_m2(27,"D");
        // выход после станка
        if ( def_rez_out==0 )
            Show_m2(29,"x");
        else
            Show_m2(29,"D");
        // длина трубы
        //
        ShowDigitZ(12,4,tube_len*10/3);
        //-----
        if ( cur_key==1 )
        {
            ClearDisplay1();
            n_screen=5;
        }
    }

    if ( n_screen==5 && xpen==0 )
    {
        // визуализация по датчикам
        xpen=1;
        // колесо
        if ( d_koleso_tr==0 )
            Show_m2(0,"_");
        else
            Show_m2(0,"К");
        // рез
        if ( rez_tr==0 )
            Show_m2(1,"_");
        else
            Show_m2(1,"Р");
        // выход после станка
        if ( def_rez_out==0 )
            Show_m2(16,"x");
        else
            Show_m2(16,"D");
        //
        // датчик 1
        if ( trub_tr_1==0 )
            Show_m2(3,"t");
        else
            Show_m2(3,"T");
        if ( dt_12==0 )
            Show_m2(4," ");
        else
            Show_m2(4,"=");
        // датчик 2
        if ( trub_tr_2==0 )
            Show_m2(5,"t");
        else
            Show_m2(5,"T");
        if ( dt_23==0 )
            Show_m2(6," ");
        else
            Show_m2(6,"=");
        // датчик 3
        if ( trub_tr_3==0 )
            Show_m2(7,"t");
        else
            Show_m2(7,"T");
        if ( dt_34==0 )
            Show_m2(8," ");
        else
            Show_m2(8,"=");
        // датчик 4
        if ( trub_tr_4==0 )
            Show_m2(9,"t");
        else
            Show_m2(9,"T");
        if ( dt_45==0 )
            Show_m2(10," ");
        else
            Show_m2(10,"=");
        // датчик 5
        if ( trub_tr_5==0 )
            Show_m2(11,"t");
        else
            Show_m2(11,"T");

        //
        if ( defect1==0 )
            Show_m2(19,"x");
        else
            Show_m2(19,"D");
        if ( defect2==0 )
            Show_m2(21,"x");
        else
            Show_m2(21,"D");
        if ( defect3==0 )
            Show_m2(23,"x");
        else
            Show_m2(23,"D");
        if ( defect4==0 )
            Show_m2(25,"x");
        else
            Show_m2(25,"D");
        if ( defect5==0 )
            Show_m2(27,"x");
        else
            Show_m2(27,"D");
        //----------------------------------
        // индикация упора
        if ( upor_flag==0 )
        {
            // упор внизу
            Show_m2(12," ");
            Show_m2(28,"I");
        }
        else
        {
            // упор внизу
            Show_m2(12,"I");
            Show_m2(28," ");
        }
        //-----
        if ( cur_key==1 )
        {
            ClearDisplay1();
            n_screen=6;
        }
    }

    if ( n_screen==6 && xpen==0 )
    {
        xpen=1;
        Show_m2(0,"Скор.");
        ShowDigitZ( 16,4,circle_speed);
        Show_m2(21,"N");
        Show_m2(7,"деф.");
        ShowDigitZ( 23,4,tube_count_def);
        Show_m2(12,"все");
        ShowDigitZ( 28,4,tube_count);
        //-----
        if ( cur_key==1 )
        {
            ClearDisplay1();
            n_screen=10;
        }
        if ( cur_key==4 )
        {
            ClearDisplay1();
            n_screen=7;
            Show_m2(2,"Сброс ?");
            timeout_key=10000;
        }
    }

    if ( n_screen==7 && xpen==0 )
    {
        // сброс
        xpen=1;
        if ( cur_key==2 )
        {
            ClearDisplay1();
            n_screen=6;
        }
        if ( cur_key==3 )
        {
            ClearDisplay1();
            tube_count=0;
            tube_count_def=0;
            n_screen=6;
        }
        if ( timeout_key==1 )
        {
            timeout_key=0;
            ClearDisplay1();
            n_screen=6;
        }
    }

    // Запрос "настрока оборудования"
    if ( n_screen==10 && xpen==0 )
    {
        xpen=1;
        ClearDisplay1();
        Show_m2( 3,"Настройка");
        Show_m2(18,"оборудования");
        n_screen=11;
        timeout_key=10000;
    }
    if ( n_screen==11 && xpen==0 )
    {
        xpen=1;
        if ( cur_key==1 )
        {
            ClearDisplay1();
            n_screen=1;
        }
        if ( cur_key==4 )
        {
            ClearDisplay1();
            n_screen=12;
            Show_m2( 4,"Пароль :");
            timeout_key=10000;
            // пароль
            psw_n=0;
            psw_summa=0;
            psw_temp=0;
        }
        if ( timeout_key==1 )
        {
            timeout_key=0;
            ClearDisplay1();
            n_screen=6;
        }
    }

    if ( n_screen==12 && xpen==0 )
    {
        xpen=1;
        ShowDigitZ( 20+psw_n,1,psw_temp);

        if ( cur_key==2 )
        {
            if ( psw_temp>0 )
                psw_temp--;
            else
                psw_temp=9;
            timeout_key=10000;
        }
        if ( cur_key==3 )
        {
            if ( psw_temp<9 )
                psw_temp++;
            else
                psw_temp=0;
            timeout_key=10000;
        }
        if ( cur_key==4 )
        {
            timeout_key=10000;
            Show_m2( 20+psw_n,"*");
            psw_n++;
            psw_summa=psw_summa*10+psw_temp;
            if ( psw_n>4 )
            {
                if ( psw_summa==32424 | psw_summa==11122 )
                {
                    n_screen=20;
                    timeout_key=20000;
                }
                else
                {
                    n_screen=13;
                    ClearDisplay1();
                    Show_m2( 2,"Ошибка ввода");
                    Show_m2(18,"пароля");
                    timeout_key=20000;
                }
            }
        }
        if ( timeout_key==1 )
        {
            timeout_key=0;
            ClearDisplay1();
            n_screen=6;
        }
    }

    if ( n_screen==13 && xpen==0 )
    {
        xpen=1;
        ShowDigitZ( 27,2,timeout_key/1000+1);
        if ( timeout_key==1 )
        {
            timeout_key=0;
            ClearDisplay1();
            n_screen=6;
        }
    }
    //--------------------------------
    // Выбор устройства для коррекции расстояния :
    if ( n_screen==20 && xpen==0 )
    {
        // xpen=1;
        ClearDisplay1();
        n_screen=21;
        dev_n=0;
        Show_m2( 0,"Расстояние до:");
    }

    if ( n_screen==21 && xpen==0 )
    {
        xpen=1;
        // название устройства
        if ( dev_n==0 )     Show_m2( 16,"Кельвина        ");// кельвин
        if ( dev_n==1 )     Show_m2( 16,"МД-32           ");// деф1
        if ( dev_n==2 )     Show_m2( 16,"толщиномера     ");// толшиномер
        if ( dev_n==3 )     dev_n++;// деф отказ
        if ( dev_n==4 )     dev_n++;// узк
        if ( dev_n==5 )     Show_m2( 16,"Дефект-2        ");
        if ( dev_n==6 )     dev_n++;
        if ( dev_n==7 )     dev_n++;
        if ( dev_n==8 )     dev_n++;
        if ( dev_n==9 )     dev_n++;
        if ( dev_n==10 )    Show_m2( 16,"линии реза      ");
        if ( dev_n==11 )    Show_m2( 16,"длн.трубы КР.3  ");
        if ( dev_n==12 )    Show_m2( 16,"длн.покрс.КР.3  ");
        if ( dev_n==13 )    Show_m2( 16,"масштаб шкалы   ");
//      if ( dev_n==13 )    dev_n++;
        if ( dev_n==14 )    Show_m2( 16,"СОХРАНИТЬ       ");
        if ( dev_n==15 )    Show_m2( 16,"ОТМЕНА          ");

        if ( cur_key==1 )
        {
            timeout_key=20000;
            dev_n++;
            if ( dev_n>15 )
                dev_n=0;
        }
        if ( cur_key==4 )
        {
            timeout_key=20000;
            // СОХРАНИТЬ
            if ( dev_n==14 )
            {
                timeout_key=2000;
                ClearDisplay1();
                Show_m2( 21,"SAVE");
                n_screen=22;
            }
            // ОТМЕНА
            if ( dev_n==15 )
            {
                timeout_key=0;
                // востоновить старые значения
                def_load();
                // kk_load();
                ClearDisplay1();
                n_screen=6;
            }
            if ( dev_n<8   )    n_screen=30;  // дефектоскопы
            if ( dev_n==10 )    n_screen=35;  // краскопульты
            if ( dev_n==11 )    n_screen=45;  // растояние до "конца" трубы
            if ( dev_n==12 )    n_screen=47;  // время покрасочного импульса
            if ( dev_n==13 )    n_screen=60;  // время покрасочного импульса
        }
        if ( timeout_key==1 )
        {
            timeout_key=0;
            // востоновить старые значения
            def_load();
            // kk_load();
            ClearDisplay1();
            n_screen=6;
        }

    }

    // дефектоскопы
    if ( n_screen==22 && xpen==0 )
    {
        xpen=1;
        if ( timeout_key==1 )
        {
            timeout_key=0;
            def_save();
            // kk_save();
            ClearDisplay1();
            n_screen=6;
        }
    }

    // дефектоскопы
    if ( n_screen==30 && xpen==0 )
    {
        xpen=1;
        ClearDisplay1();
        if ( dev_n==0 )     Show_m2( 0,"Кельвин         ");
        if ( dev_n==1 )     Show_m2( 0,"МД-32           ");
        if ( dev_n==2 )     Show_m2( 0,"Толщиномер      ");
        if ( dev_n==5 )     Show_m2( 0,"Дефект-2        ");
        dev_l=d_def_sm[dev_n];
        ShowDigitZ(24,4,dev_l);
        Show_m2( 28,"n");
        {
            unsigned int tmp;
            unsigned int tmp1;
            unsigned int tmp2;
            tmp=dev_l*10/3;
            tmp1=tmp/100;
            tmp2=tmp-tmp1*100;
            ShowDigitZ(16,2,tmp1);
            Show_m2( 18,".");
            ShowDigitZ( 19,2,tmp2 );
            Show_m2( 21,"M");
        }
        n_screen=40;
        auto_repeat(1);
    }

    // редактирование
    if ( n_screen==40 && xpen==0 )
    {
        unsigned int tmp;
        unsigned int tmp1;
        unsigned int tmp2;
        xpen=1;
        if ( cur_key==1 )   // отмена
        {
            timeout_key=0;
            // востоновить старые значения
            def_load();
            // kk_load();
            ClearDisplay1();
            n_screen=6;
            auto_repeat(0);
        }
        tmp=0;
        if ( cur_key==2 )   // -
        {
            timeout_key=20000;
            tmp=1;
            if ( dev_l>30 )
                dev_l--;
        }
        if ( cur_key==3 )   // +
        {
            timeout_key=20000;
            tmp=1;
            if ( dev_l<1800 )
                dev_l++;
        }
        // индикация
        if ( tmp>0 )
        {
            ShowDigitZ(24,4,dev_l);
            tmp=dev_l*10/3;
            tmp1=tmp/100;
            tmp2=tmp-tmp1*100;
            ShowDigitZ(16,2,tmp1);
            Show_m2( 18,".");
            ShowDigitZ( 19,2,tmp2 );
            Show_m2( 21,"M");
        }

        if ( cur_key==4 )   // потверждение
        {
            // сохранение растояния до дефектоскопов
            if ( dev_n==0 )  // Кельвин
            {
                d_def_sm[0]=dev_l;
                //
                ClearDisplay1();
                timeout_key=0;
                n_screen=20;
                auto_repeat(0);
            }
            if ( dev_n==1 )  // МД-32
            {
                // дефект1
                d_def_sm[1]=dev_l;
                // отказ
                d_def_sm[3]=dev_l;
                //
                ClearDisplay1();
                timeout_key=0;
                n_screen=20;
            }
            if ( dev_n==2 )  // толшиномер
            {
                // больше
                d_def_sm[2]=dev_l;
                // меньше
                d_def_sm[4]=dev_l;
                ClearDisplay1();
                timeout_key=0;
                n_screen=20;
            }

            if ( dev_n==5 )  // стыколов PC-1
            {
                d_def_sm[5]=dev_l;
                //
                ClearDisplay1();
                timeout_key=0;
                n_screen=20;
            }
            if ( dev_n==10 ) // линия реза
            {
                d_rez_sm=dev_l;
                ClearDisplay1();
                timeout_key=0;
                n_screen=20;
            }
        }

        // тайм аут
        if ( timeout_key==1 )
        {
            timeout_key=0;
            // востоновить старые значения
            def_load();
            // kk_load();
            ClearDisplay1();
            n_screen=6;
            auto_repeat(0);
        }
    }

    if ( n_screen==35 && xpen==0 )
    {
        xpen=1;
        ClearDisplay1();
        Show_m2( 0,"до линии реза   ");
        dev_l=d_rez_sm;
        ShowDigitZ(24,4,dev_l);
        Show_m2( 28,"n");
        {
            unsigned int tmp;
            unsigned int tmp1;
            unsigned int tmp2;
            tmp=dev_l*10/3;
            tmp1=tmp/100;
            tmp2=tmp-tmp1*100;
            ShowDigitZ(16,2,tmp1);
            Show_m2( 18,".");
            ShowDigitZ( 19,2,tmp2 );
            Show_m2( 21,"M");
        }
        n_screen=40;
    }

    // время - растояние до конца трубы
    if ( n_screen==45 && xpen==0 )
    {
        xpen=1;
        ClearDisplay1();
        Show_m2( 0,"длина трубы");
        n_screen=46;
        kr3_temp = kr3_zd_const;
        auto_repeat(1);
    }

    if ( n_screen==46 && xpen==0 )
    {
        xpen=1;
        ShowDigitZ(16,5,kr3_temp);
        Show_m2( 22,"мСек.");
        if ( cur_key==2 )
        {
            timeout_key=20000;
            // if ( kr3_temp>1000 ) kr3_temp--;
            ParametrDown( &kr3_temp, sizeof(kr3_temp), 5000, 15000, 0 );
        }
        if ( cur_key==3 )
        {
            timeout_key=20000;
            // if ( kr3_temp<10000 ) kr3_temp++;
            ParametrUp( &kr3_temp, sizeof(kr3_temp), 5000, 15000, 0 );
        }
        if ( cur_key==4 )
        {
            timeout_key=20000;
            auto_repeat(0);
            kr3_zd_const = kr3_temp;
            n_screen=20;
        }
    }

    if ( n_screen==47 && xpen==0 )
    {
        xpen=1;
        ClearDisplay1();
        Show_m2( 0,"время покраски");
        n_screen=48;
        kr3_temp = kr3_dl_const;
        auto_repeat(1);
    }

    if ( n_screen==48 && xpen==0 )
    {
        xpen=1;
        ShowDigitZ(16,4,kr3_temp);
        Show_m2( 21,"мСек.");
        if ( cur_key==2 )
        {
            timeout_key=20000;
            // if ( kr3_temp>1000 ) kr3_temp--;
            ParametrDown( &kr3_temp, sizeof(kr3_temp), 300, 3000, 0 );
        }
        if ( cur_key==3 )
        {
            timeout_key=20000;
            // if ( kr3_temp<10000 ) kr3_temp++;
            ParametrUp( &kr3_temp, sizeof(kr3_temp), 300, 3000, 0 );
        }
        if ( cur_key==4 )
        {
            timeout_key=20000;
            auto_repeat(0);
            kr3_dl_const = kr3_temp;
            n_screen=20;
        }
    }

    // --- масштаб
    if ( n_screen==60 && xpen==0 )
    {
        xpen=1;
        ClearDisplay1();
        Show_m2( 0,"масштаб шкалы");
        Show_m2( 18,".");
        Show_m2( 20,"m");
        Show_m2( 25,"имп/д");
        n_screen=61;
        kr3_temp = ind_def_k;
        auto_repeat(1);
    }
    if ( n_screen==61 && xpen==0 )
    {
        xpen=1;
        int o = kr3_temp*12/3;
        ShowDigitZ(16,2,o/10);
        ShowDigitZ(19,1,o%10);
        ShowDigitZ(22,3,kr3_temp);
        if ( cur_key==2 )
        {
            timeout_key=20000;
            ParametrDown( &kr3_temp, sizeof(kr3_temp), 5, 255, 0 );
        }
        if ( cur_key==3 )
        {
            timeout_key=20000;
            ParametrUp( &kr3_temp, sizeof(kr3_temp), 5, 255, 0 );
        }
        if ( cur_key==4 )
        {
            timeout_key=20000;
            auto_repeat(0);
            ind_def_k = kr3_temp;
            n_screen=20;
        }
    }

}

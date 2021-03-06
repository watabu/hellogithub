#include "DxLib.h"
#define width 1920
#define height 1080

class Mino{
private:
	int mino[15] = {};// ミノの種類は０から６の７種類、１から１４が次のミノ
public:
	void shuffleMino();//次のミノリストをシャッフルする

	int getNextMino(); //次のミノを返す
	void DrawMino();//テスト用
};

int WINAPI WinMain(HINSTANCE, HINSTANCE, LPSTR, int) {

	//ChangeWindowMode(TRUE);
	SetGraphMode(width, height, 16), SetMainWindowText("Tetorinu");
	DxLib_Init();
	SetDrawScreen(DX_SCREEN_BACK);


	Mino mino;
	mino.shuffleMino();
	int flag = 0;
	int mino2 = mino.getNextMino();
	while (ScreenFlip() == 0 && ProcessMessage() == 0 && ClearDrawScreen() == 0 && CheckHitKey(KEY_INPUT_ESCAPE) == 0) {
	
		if (CheckHitKey(KEY_INPUT_SPACE) != 0) {
			if (flag == 0) {
				mino2 = mino.getNextMino();
			}
			flag = 1;
		}
		else {
			flag = 0;
		}
	
		
		DrawFormatString(100, 100, GetColor(255, 255, 0), "mino is %d", mino2);
		mino.DrawMino();
	}
	DxLib_End();
	return 0;
}



void Mino::shuffleMino() {
	int flag[15] = {};//代入されてないなら０
	int num;
	for (int i = 1; i < 15;) {
		num = GetRand(13) + 1;//１〜14まで
		if (flag[num] == 0) {//代入されてなければ
			mino[num] = i % 7;//種類を代入　２種類ずつ配置される
			flag[num] = 1;
			i++;
		}
	}
}

int Mino::getNextMino() {
	static int num = 0;
	num++;
	if (num > 14) {
		num = 1;
		shuffleMino();

	}
	return mino[num];//ミノの種類を返す

}

void Mino::DrawMino() {
	for (int i = 1; i < 15; i++) {
		DrawFormatString(200, 100 + 30 * i, GetColor(255, 255, 0), "Mino %d is %d", i, mino[i]);
	}
}
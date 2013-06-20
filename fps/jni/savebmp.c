/********************************************************************
	created:	2012/02/07
	filename: 	savebmp.c
	author:		
	
	purpose:	
*********************************************************************/

#include <stdlib.h>
#include <stdio.h>
#include <memory.h>


//-------------------------------------------------------------------
/*
����λͼ�ļ������
�����ṹ���� �� ��
����λͼ�ļ�ͷ (bitmap-file header) BITMAPFILEHEADER bmfh
����λͼ��Ϣͷ (bitmap-information header) BITMAPINFOHEADER bmih
������ɫ��(color table) RGBQUAD aColors[]
����ͼ�����������ֽ� BYTE aBitmapBits[]
*/
typedef struct bmp_header 
{
	short twobyte			;//�����ֽڣ�������֤�����Ա�������У��������ַ�����д���ļ���
	//14B
	char bfType[2]			;//!�ļ�������,��ֵ������0x4D42��Ҳ�����ַ�'BM'
	unsigned int bfSize		;//!˵���ļ��Ĵ�С�����ֽ�Ϊ��λ
	unsigned int bfReserved1;//��������������Ϊ0
	unsigned int bfOffBits	;//!˵�����ļ�ͷ��ʼ��ʵ�ʵ�ͼ������֮����ֽڵ�ƫ����������Ϊ14B+sizeof()
}BMPHEADER;

typedef struct bmp_info
{
	//40B
	unsigned int biSize			;//!BMPINFO�ṹ����Ҫ������
	int biWidth					;//!ͼ��Ŀ�ȣ�������Ϊ��λ
	int biHeight				;//!ͼ��Ŀ�ȣ�������Ϊ��λ,�����ֵ��������˵��ͼ���ǵ���ģ������ֵ�Ǹ��������������
	unsigned short biPlanes		;//!Ŀ���豸˵��λ��������ֵ�����Ǳ���Ϊ1
	unsigned short biBitCount	;//!������/���أ���ֵΪ1��4��8��16��24����32
	unsigned int biCompression	;//˵��ͼ������ѹ��������
#define BI_RGB        0L	//û��ѹ��
#define BI_RLE8       1L	//ÿ������8���ص�RLEѹ�����룬ѹ����ʽ��2�ֽ���ɣ��ظ����ؼ�������ɫ��������
#define BI_RLE4       2L	//ÿ������4���ص�RLEѹ�����룬ѹ����ʽ��2�ֽ����
#define BI_BITFIELDS  3L	//ÿ�����صı�����ָ�������������
	unsigned int biSizeImage	;//ͼ��Ĵ�С�����ֽ�Ϊ��λ������BI_RGB��ʽʱ��������Ϊ0
	int biXPelsPerMeter			;//ˮƽ�ֱ��ʣ�������/�ױ�ʾ
	int biYPelsPerMeter			;//��ֱ�ֱ��ʣ�������/�ױ�ʾ
	unsigned int biClrUsed		;//λͼʵ��ʹ�õĲ�ɫ���е���ɫ����������Ϊ0�Ļ�����˵��ʹ�����е�ɫ���
	unsigned int biClrImportant	;//��ͼ����ʾ����ҪӰ�����ɫ��������Ŀ�������0����ʾ����Ҫ��
}BMPINFO;

typedef struct tagRGBQUAD {
	unsigned char rgbBlue;
	unsigned char rgbGreen;
	unsigned char rgbRed;
	unsigned char rgbReserved;
} RGBQUAD;

typedef struct tagBITMAPINFO {
    BMPINFO    bmiHeader;
    //RGBQUAD    bmiColors[1];
	unsigned int rgb[3];
} BITMAPINFO;

typedef struct
{
	BMPINFO bmiHeader;
	unsigned int rgb[4];
}RGBX8888INFO;

static int get_rgb888_header(int w, int h, BMPHEADER * head, BMPINFO * info)
{
	int size = 0;
	if (head && info) {
		size = w * h * 3;
		memset(head, 0, sizeof(* head));
		memset(info, 0, sizeof(* info));
		head->bfType[0] = 'B';
		head->bfType[1] = 'M';
		head->bfOffBits = 14 + sizeof(* info);
		head->bfSize = head->bfOffBits + size;
		head->bfSize = (head->bfSize + 3) & ~3;
		size = head->bfSize - head->bfOffBits;
		
		info->biSize = sizeof(BMPINFO);
		info->biWidth = w;
		info->biHeight = -h;
		info->biPlanes = 1;
		info->biBitCount = 24;
		info->biCompression = BI_RGB;
		info->biSizeImage = size;

		printf("rgb888:%dbit,%d*%d,%d\n", info->biBitCount, w, h, head->bfSize);
	}
	return size;
}

static int get_rgb565_header(int w, int h, BMPHEADER * head, BITMAPINFO * info)
{
	int size = 0;
	if (head && info) {
		size = w * h * 2;
		memset(head, 0, sizeof(* head));
		memset(info, 0, sizeof(* info));
		head->bfType[0] = 'B';
		head->bfType[1] = 'M';
		head->bfOffBits = 14 + sizeof(* info);
		head->bfSize = head->bfOffBits + size;
		head->bfSize = (head->bfSize + 3) & ~3;
		size = head->bfSize - head->bfOffBits;
		
		info->bmiHeader.biSize = sizeof(info->bmiHeader);
		info->bmiHeader.biWidth = w;
		info->bmiHeader.biHeight = -h;
		info->bmiHeader.biPlanes = 1;
		info->bmiHeader.biBitCount = 16;
		info->bmiHeader.biCompression = BI_BITFIELDS;
		info->bmiHeader.biSizeImage = size;

		info->rgb[0] = 0xF800;
		info->rgb[1] = 0x07E0;
		info->rgb[2] = 0x001F;

		printf("rgb565:%dbit,%d*%d,%d\n", info->bmiHeader.biBitCount, w, h, head->bfSize);
	}
	return size;
}

static int get_rgbx8888_header(int w, int h, BMPHEADER * head, RGBX8888INFO * info)
{
	int size = 0;
	if (head && info) {
		size = w * h * 4;
		memset(head, 0, sizeof(* head));
		memset(info, 0, sizeof(* info));
		head->bfType[0] = 'B';
		head->bfType[1] = 'M';
		head->bfOffBits = 14 + sizeof(* info);
		head->bfSize = head->bfOffBits + size;
		head->bfSize = (head->bfSize + 3) & ~3;
		size = head->bfSize - head->bfOffBits;
		
		info->bmiHeader.biSize = sizeof(info->bmiHeader);
		info->bmiHeader.biWidth = w;
		info->bmiHeader.biHeight = -h;
		info->bmiHeader.biPlanes = 1;
		info->bmiHeader.biBitCount = 32;
		info->bmiHeader.biCompression = BI_BITFIELDS;
		info->bmiHeader.biSizeImage = size;
		
		info->rgb[0] = 0x00FF0000;
		info->rgb[1] = 0x0000FF00;
		info->rgb[2] = 0x000000FF;
		info->rgb[3] = 0;

		printf("rgbx8888:%dbit,%d*%d,%d\n", info->bmiHeader.biBitCount, w, h, head->bfSize);
	}
	return size;
}

static int save_bmp_rgb565(FILE * hfile, int w, int h, void * pdata)
{
	int success = 0;
	int size = 0;
	BMPHEADER head;
	BITMAPINFO info;
	
	size = get_rgb565_header(w, h, &head, &info);
	if (size > 0) {
		fwrite(head.bfType, 1, 14, hfile);
		fwrite(&info, 1, sizeof(info), hfile);
		fwrite(pdata, 1, size, hfile);
		success = 1;
	}

	return success;
}

static int save_bmp_rgb888(FILE * hfile, int w, int h, void * pdata)
{
	int success = 0;
	int size = 0;
	BMPHEADER head;
	BMPINFO info;
	
	size = get_rgb888_header(w, h, &head, &info);
	if (size > 0) {
		fwrite(head.bfType, 1, 14, hfile);
		fwrite(&info, 1, sizeof(info), hfile);
		fwrite(pdata, 1, size, hfile);
		success = 1;
	}
	
	return success;
}

static int save_bmp_rgbx8888(FILE * hfile, int w, int h, void * pdata)
{
	int success = 0;
	int size = 0;
	BMPHEADER head;
	RGBX8888INFO info;
	
	size = get_rgbx8888_header(w, h, &head, &info);
	if (size > 0) {
		fwrite(head.bfType, 1, 14, hfile);
		fwrite(&info, 1, sizeof(info), hfile);
		fwrite(pdata, 1, size, hfile);
		success = 1;
	}
	
	return success;
}

int save_bmp(const char * path, int w, int h, void * pdata, int bpp)
{
	int success = 0;
	FILE * hfile = NULL;

	do 
	{
		if (path == NULL || w <= 0 || h <= 0 || pdata == NULL) {
			printf("if (path == NULL || w <= 0 || h <= 0 || pdata == NULL)\n");
			break;
		}

		remove(path);
		hfile = fopen(path, "wb");
		if (hfile == NULL) {
			printf("open(%s) failed!\n", path);
			break;
		}

		switch (bpp)
		{
		case 16:
			success = save_bmp_rgb565(hfile, w, h, pdata);
			break;
		case 24:
			success = save_bmp_rgb888(hfile, w, h, pdata);
			break;
		case 32:
			success = save_bmp_rgbx8888(hfile, w, h, pdata);
			break;
		default:
			printf("error: not support format:%d!\n",bpp);
			success = 0;
			break;
		}
	} while (0);

	if (hfile != NULL)
		fclose(hfile);
	
	return success;
}

//-------------------------------------------------------------------

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
　　位图文件的组成
　　结构名称 符 号
　　位图文件头 (bitmap-file header) BITMAPFILEHEADER bmfh
　　位图信息头 (bitmap-information header) BITMAPINFOHEADER bmih
　　彩色表　(color table) RGBQUAD aColors[]
　　图象数据阵列字节 BYTE aBitmapBits[]
*/
typedef struct bmp_header 
{
	short twobyte			;//两个字节，用来保证下面成员紧凑排列，这两个字符不能写到文件中
	//14B
	char bfType[2]			;//!文件的类型,该值必需是0x4D42，也就是字符'BM'
	unsigned int bfSize		;//!说明文件的大小，用字节为单位
	unsigned int bfReserved1;//保留，必须设置为0
	unsigned int bfOffBits	;//!说明从文件头开始到实际的图象数据之间的字节的偏移量，这里为14B+sizeof()
}BMPHEADER;

typedef struct bmp_info
{
	//40B
	unsigned int biSize			;//!BMPINFO结构所需要的字数
	int biWidth					;//!图象的宽度，以象素为单位
	int biHeight				;//!图象的宽度，以象素为单位,如果该值是正数，说明图像是倒向的，如果该值是负数，则是正向的
	unsigned short biPlanes		;//!目标设备说明位面数，其值将总是被设为1
	unsigned short biBitCount	;//!比特数/象素，其值为1、4、8、16、24、或32
	unsigned int biCompression	;//说明图象数据压缩的类型
#define BI_RGB        0L	//没有压缩
#define BI_RLE8       1L	//每个象素8比特的RLE压缩编码，压缩格式由2字节组成（重复象素计数和颜色索引）；
#define BI_RLE4       2L	//每个象素4比特的RLE压缩编码，压缩格式由2字节组成
#define BI_BITFIELDS  3L	//每个象素的比特由指定的掩码决定。
	unsigned int biSizeImage	;//图象的大小，以字节为单位。当用BI_RGB格式时，可设置为0
	int biXPelsPerMeter			;//水平分辨率，用象素/米表示
	int biYPelsPerMeter			;//垂直分辨率，用象素/米表示
	unsigned int biClrUsed		;//位图实际使用的彩色表中的颜色索引数（设为0的话，则说明使用所有调色板项）
	unsigned int biClrImportant	;//对图象显示有重要影响的颜色索引的数目，如果是0，表示都重要。
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

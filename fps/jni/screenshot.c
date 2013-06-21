/********************************************************************
	created:	2012/02/07
	filename: 	screenshot.c
	author:		
	
	purpose:	
*********************************************************************/

#include <stdlib.h>
#include <memory.h>
#include <string.h>
#include <stdio.h>
#include <unistd.h>
#include "./myfb.h"
#include <sys/time.h>
#include<math.h>

#ifndef WIN32
//-------------------------------------------------------------------

int screen_shot()
{
	struct FB * fb = NULL;
	fb = fb_create();
	if (fb) {
		int i=0;
		int width = fb_width(fb);
		int height = fb_height(fb);
		int bpp = fb_bpp(fb);
		bpp = bpp/8;
		int size = width*height*bpp;
		
		int offset_fb = fb_offset(fb);
		int virtual_size = fb_virtual_size(fb);
		
		printf("%d,%d,%d,%d,%d,%d\n",width,height,bpp,size,offset_fb,virtual_size);
		void * framebit = fb_bits(fb);
		
		struct timeval start, end;
		gettimeofday( &start, NULL );
		int offset = 80*width*bpp;
		int intervaltime = 15;
		int intervaltime_u = intervaltime*1000;
		printf("%d\n",intervaltime_u);
		
		void * currentImageRaw;
		void * preImageRaw;
		int compareSize = size-offset;
        char * imgbuffer = (char *)malloc(size);
		//framebit = fb_bits(fb);	
		//currentImageRaw = framebit + offset;
		//int frameoffset = fb_offset(fb);
        //memcpy(imgbuffer,currentImageRaw,size - offset);
		
		int refreshFlag = 1;
		int count = 0;
		int diffBuffer[60] ;
		memset(diffBuffer,0,60*sizeof(int));
		int startTime = 0;
		while(1)
		{
			usleep(intervaltime_u);
			gettimeofday( &end, NULL );
			int timeuse = 1000000 * ( end.tv_sec - start.tv_sec ) + end.tv_usec - start.tv_usec;
			int currentTime = timeuse/1000;
			printf("time: %d :::::", currentTime);
			
			framebit = fb_bits(fb);	
			//int frameoffset = fb_offset(fb);
			//printf("offset: %d :::::", frameoffset);
			currentImageRaw = framebit + offset;
			//preImageRaw = framebit+offset+size;
			int diffpercent = compareImage(imgbuffer,currentImageRaw,compareSize);	
			printf("diff: %d :::::\n",diffpercent);
			
			diffBuffer[count] = diffpercent;
			int spanTime = currentTime-startTime;
			if(spanTime>1000){
				int frames = getChangeBuffer(diffBuffer,60);
				printf("time: %d :::::", currentTime);
				printf("frames: %d :::::\n", frames);
				count = 0;
				startTime = currentTime;
				memset(diffBuffer,0,60*sizeof(int));
			}
			count++;
		}
		fb_destory(fb);
		free(imgbuffer);
	}
	return 0;
}

int getChangeBuffer(int *buffer, int size){
	int count = 0;
	while(--size >=0){		
		if(buffer[size]!=0)
			count++;
	}
	return count;
}

int compareImage(void *dest,void *src,int size){
    int * f = (int *)dest;
	int * r = (int *)src;
    int result = 0;
	int fl;
	int rl;	
	int count = size/4-1;
	int step = 53;
	f = f + count;
	r = r + count;
	while(*f == *r && count > 0){
		count -= step;
		if(count>0){
			f -= step;
			r -= step;
		}
	}
	if(*f != *r)
		result = (*f - *r);//<<25 ? (*f - *r)<<25 : 10;
    while(count > 0) {
        *f = *r;
		count -= step;
		if(count > 0){
			f -= step;
			r -= step;
		}
    }
	return result;
}

int copyImage(void *dest,void *src,int size){
/*
    unsigned short * f = (unsigned short *)dest;
	unsigned short * r = (unsigned short *)src;
	int count = size/4;
	int step = 133;
	//f = f + count;
	//r = r + count;
	while(count > 0){
		count = count - step;
		f = f + step;
		r = r + step;
		*f = *r;
	}
	*/
	char* dst8 = (char*)dest;
    char* src8 = (char*)src;

	while (size--) {
		*dst8++ = *src8++;
	}	
	return 0;
}

//-------------------------------------------------------------------
#else //#ifndef WIN32

int screen_shot(const char * path)
{
	int w = 80;
	int h = 20;
	int size = w*h*2;
	char * buf = (char *)malloc(size + 10);
	
	if (buf) {
		memset(buf, 0xff, (size + 10));
		memset(buf, 0xff, size/2);
		memset(buf + size/2, 0x00, size/2);
		save_bmp("./jni/w.bmp", w, h, buf, 16);
		free(buf);
	}
	
	return 0;
}

#endif //#ifndef WIN32
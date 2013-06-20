/********************************************************************
	created:	2012/02/07
	filename: 	main.c
	author:		
	
	purpose:	
*********************************************************************/

//-------------------------------------------------------------------

#include <stdio.h>
#include "./myfb.h"

int main(int argc, char * argv[])
{
	int intervaltime = atoi(argv[1]);
	
	//printf("main enter!%d %d\n",intervaltime,argc);
	screen_shot(intervaltime);
	
	//printf("main exit!\n");
	
	return 0;
}

//-------------------------------------------------------------------

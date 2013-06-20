LOCAL_PATH:= $(call my-dir)

include $(CLEAR_VARS)
LOCAL_MODULE:= libsavebmp
LOCAL_SRC_FILES:=myfb.c savebmp.c screenshot.c
LOCAL_STATIC_LIBRARIES := libcutils libc
include $(BUILD_STATIC_LIBRARY)

################################################

include $(CLEAR_VARS)
LOCAL_MODULE:= save
LOCAL_SRC_FILES:=main.c
LOCAL_STATIC_LIBRARIES := libsavebmp libcutils libc
include $(BUILD_EXECUTABLE)

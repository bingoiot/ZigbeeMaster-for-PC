# ZStack Home Automation for jifan evaluation board by Texas Instruments Z-Stack1.2.2a

* ZigbeeMaster.exe 用于测试Zigbee协调器程序。如果你想要获取ZigbeeMaster的源码请打开https://github.com/bingoiot/ZigbeeMaster-for-PC

* 程序仅用于开发板的评估使用，没有做过任何优化。如果你想让你的zigbee程序拥有更好的体验请联系我们www.afteriot.com/

# 评估板程序发现存在以下两个问题：

* 定期唤醒的终端设备在加入网络后，如长期无法找到其父节点，有机会退出原来的网络等待再次入网。

* 定期唤醒的终端设备在两个父节点切换过程中，如其中一个父节点  
因断电或复位的原因将导致终端设备切换父节点，当父节点再次正  
常工作，将有可能导致终端设备无法再次进行双向通信。

![image](https://github.com/bingoiot/EV-Z-Stack-HA-1.2.2a/blob/master/img/img1.png)

![image](https://github.com/bingoiot/EV-Z-Stack-HA-1.2.2a/blob/master/img/img2.png)

![image](https://github.com/bingoiot/EV-Z-Stack-HA-1.2.2a/blob/master/img/img3.png)

![image](https://github.com/bingoiot/EV-Z-Stack-HA-1.2.2a/blob/master/img/img4.png)
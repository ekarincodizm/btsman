/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50141
Source Host           : localhost:3306
Source Database       : bts

Target Server Type    : MYSQL
Target Server Version : 50141
File Encoding         : 65001

Date: 2010-06-14 01:00:12
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `authorization`
-- ----------------------------
DROP TABLE IF EXISTS `authorization`;
CREATE TABLE `authorization` (
  `auth_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `role_id` int(10) unsigned NOT NULL,
  `rightname` varchar(100) NOT NULL,
  `action` varchar(50) NOT NULL,
  PRIMARY KEY (`auth_id`),
  KEY `IDX_UNQ_authorize_name` (`role_id`) USING BTREE,
  CONSTRAINT `FK_auth_role_id` FOREIGN KEY (`role_id`) REFERENCES `role` (`role_id`)
) ENGINE=InnoDB AUTO_INCREMENT=71 DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of authorization
-- ----------------------------
INSERT INTO `authorization` VALUES ('1', '1', 'Administrator.aspx', '*');
INSERT INTO `authorization` VALUES ('2', '1', 'BranchManage.aspx', '*');
INSERT INTO `authorization` VALUES ('3', '1', 'CourseManage.aspx', '*');
INSERT INTO `authorization` VALUES ('4', '1', 'Default.aspx', '*');
INSERT INTO `authorization` VALUES ('5', '1', 'Home.aspx', '*');
INSERT INTO `authorization` VALUES ('6', '1', 'PaidGroupManage.aspx', '*');
INSERT INTO `authorization` VALUES ('7', '1', 'PaymentManage.aspx', '*');
INSERT INTO `authorization` VALUES ('8', '1', 'PromotionManage.aspx', '*');
INSERT INTO `authorization` VALUES ('9', '1', 'RegisterCourse.aspx', '*');
INSERT INTO `authorization` VALUES ('10', '1', 'RegistrationManage.aspx', '*');
INSERT INTO `authorization` VALUES ('11', '1', 'ReportDailyRegistration.aspx', '*');
INSERT INTO `authorization` VALUES ('12', '1', 'ReportRegis.aspx', '*');
INSERT INTO `authorization` VALUES ('13', '1', 'RoomManage.aspx', '*');
INSERT INTO `authorization` VALUES ('14', '1', 'StudentManage.aspx', '*');
INSERT INTO `authorization` VALUES ('15', '1', 'TeacherManage.aspx', '*');
INSERT INTO `authorization` VALUES ('16', '2', 'Administrator.aspx', 'default');
INSERT INTO `authorization` VALUES ('17', '2', 'Administrator.aspx', 'list');
INSERT INTO `authorization` VALUES ('18', '2', 'BranchManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('19', '2', 'BranchManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('20', '2', 'CourseManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('21', '2', 'CourseManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('22', '2', 'Default.aspx', '*');
INSERT INTO `authorization` VALUES ('23', '2', 'Home.aspx', '*');
INSERT INTO `authorization` VALUES ('24', '2', 'PaidGroupManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('25', '2', 'PaidGroupManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('26', '2', 'PaymentManage.aspx', '*');
INSERT INTO `authorization` VALUES ('27', '2', 'PromotionManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('28', '2', 'PromotionManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('29', '2', 'RegisterCourse.aspx', '*');
INSERT INTO `authorization` VALUES ('30', '2', 'RegistrationManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('31', '2', 'RegistrationManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('32', '2', 'RegistrationManage.aspx', 'init_print_card');
INSERT INTO `authorization` VALUES ('33', '2', 'RegistrationManage.aspx', 'init_print_receipt');
INSERT INTO `authorization` VALUES ('34', '2', 'RegistrationManage.aspx', 'init_print_all');
INSERT INTO `authorization` VALUES ('35', '2', 'ReportDailyRegistration.aspx', '*');
INSERT INTO `authorization` VALUES ('36', '2', 'ReportRegis.aspx', '*');
INSERT INTO `authorization` VALUES ('37', '2', 'RoomManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('38', '2', 'RoomManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('39', '2', 'StudentManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('40', '2', 'StudentManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('41', '2', 'StudentManage.aspx', 'add');
INSERT INTO `authorization` VALUES ('42', '2', 'StudentManage.aspx', 'edit');
INSERT INTO `authorization` VALUES ('43', '2', 'StudentManage.aspx', 'view');
INSERT INTO `authorization` VALUES ('44', '2', 'TeacherManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('45', '2', 'TeacherManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('46', '3', 'Administrator.aspx', 'default');
INSERT INTO `authorization` VALUES ('47', '3', 'Administrator.aspx', 'list');
INSERT INTO `authorization` VALUES ('48', '3', 'BranchManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('49', '3', 'BranchManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('50', '3', 'CourseManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('51', '3', 'CourseManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('52', '3', 'Default.aspx', '*');
INSERT INTO `authorization` VALUES ('53', '3', 'Home.aspx', '*');
INSERT INTO `authorization` VALUES ('54', '3', 'PromotionManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('55', '3', 'PromotionManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('56', '3', 'RegisterCourse.aspx', '*');
INSERT INTO `authorization` VALUES ('57', '3', 'RegistrationManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('58', '3', 'RegistrationManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('59', '3', 'RegistrationManage.aspx', 'init_print_card');
INSERT INTO `authorization` VALUES ('60', '3', 'RegistrationManage.aspx', 'init_print_receipt');
INSERT INTO `authorization` VALUES ('61', '3', 'RegistrationManage.aspx', 'init_print_all');
INSERT INTO `authorization` VALUES ('62', '3', 'RoomManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('63', '3', 'RoomManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('64', '3', 'StudentManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('65', '3', 'StudentManage.aspx', 'list');
INSERT INTO `authorization` VALUES ('66', '3', 'StudentManage.aspx', 'add');
INSERT INTO `authorization` VALUES ('67', '3', 'StudentManage.aspx', 'edit');
INSERT INTO `authorization` VALUES ('68', '3', 'StudentManage.aspx', 'view');
INSERT INTO `authorization` VALUES ('69', '3', 'TeacherManage.aspx', 'default');
INSERT INTO `authorization` VALUES ('70', '3', 'TeacherManage.aspx', 'list');

-- ----------------------------
-- Table structure for `branch`
-- ----------------------------
DROP TABLE IF EXISTS `branch`;
CREATE TABLE `branch` (
  `branch_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `branch_name` varchar(100) NOT NULL,
  `address` varchar(200) NOT NULL,
  `tel` varchar(45) NOT NULL,
  `supervisor` varchar(100) NOT NULL,
  `img` varchar(50) NOT NULL DEFAULT 'noimg.jpg',
  PRIMARY KEY (`branch_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=tis620 PACK_KEYS=1;

-- ----------------------------
-- Records of branch
-- ----------------------------
INSERT INTO `branch` VALUES ('1', 'BTS สีลม', 'อ.ธนิยะ ถ.สีลม', ' 02 - 632 - 9884-5 Fax:. 02 - 632 - 9965', ' นายสมชาย แซ่แต้', '62232900.jpg');
INSERT INTO `branch` VALUES ('2', 'BTS สยาม', 'สยามแสควร์ ซ.2', '02 - 632 - 9884-5 Fax:. 02 - 632 - 9965', ' นางสมจิต แซ่ยาว', '488588370.jpg');
INSERT INTO `branch` VALUES ('3', 'บ้านไอ่พง', 'ดินแดง', '', '', 'noimg.jpg');

-- ----------------------------
-- Table structure for `btsconfig`
-- ----------------------------
DROP TABLE IF EXISTS `btsconfig`;
CREATE TABLE `btsconfig` (
  `name` varchar(50) NOT NULL,
  `value` varchar(1000) CHARACTER SET latin1 DEFAULT NULL,
  PRIMARY KEY (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of btsconfig
-- ----------------------------
INSERT INTO `btsconfig` VALUES ('LOG_SEVERITY', 'INFO');

-- ----------------------------
-- Table structure for `course`
-- ----------------------------
DROP TABLE IF EXISTS `course`;
CREATE TABLE `course` (
  `course_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `bts_course_id` varchar(50) NOT NULL,
  `course_name` varchar(100) NOT NULL,
  `short_name` varchar(50) NOT NULL,
  `course_desc` varchar(1000) NOT NULL,
  `room_id` int(10) unsigned NOT NULL,
  `teacher_id` int(10) unsigned NOT NULL,
  `category` varchar(50) NOT NULL,
  `start_date` date NOT NULL,
  `end_date` date NOT NULL,
  `day_of_week` varchar(3) NOT NULL COMMENT 'Mon,Tue,Wed,Thu,Fri,Sat,Sun',
  `start_time` time NOT NULL,
  `end_time` time NOT NULL,
  `cost` int(10) unsigned NOT NULL DEFAULT '0',
  `seat_limit` int(11) NOT NULL DEFAULT '40',
  `bank_regis_limit` int(11) NOT NULL DEFAULT '30',
  `image` varchar(50) DEFAULT NULL,
  `is_active` int(4) NOT NULL DEFAULT '1',
  `is_paid` int(11) NOT NULL DEFAULT '0' COMMENT '0=notpaid 1=paid',
  PRIMARY KEY (`course_id`),
  UNIQUE KEY `UNQ_bts_course_id` (`bts_course_id`)
) ENGINE=InnoDB AUTO_INCREMENT=85 DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of course
-- ----------------------------
INSERT INTO `course` VALUES ('1', 'BTS0001', 'ภาษาไทย ม.ต้น', 'ไทยม.ต้น', 'เสริมสร้างความรู้ภาษไทยสำหรับนักเรียนม.1-3', '2', '1', 'THA', '2010-01-20', '2010-02-24', 'Mon', '10:00:00', '12:00:00', '4000', '40', '0', 'noimg.jpg', '1', '0');
INSERT INTO `course` VALUES ('2', 'BTS0002', 'คณิตรศาสตร์ม.ปลาย', 'คณิตม.ปลาย', 'เสริมสร้างความรู้คณิตศาสตร์ม.ปลาย สำหรับเตรียมเอ็น', '4', '2', 'MTH', '2010-02-10', '2010-03-20', 'Mon', '13:00:00', '17:00:00', '5000', '50', '0', 'noimg.jpg', '1', '0');
INSERT INTO `course` VALUES ('3', 'BTS0003', 'เตรียมเอ็น', 'เตรียมเอ็น', 'เตรียมเอ็น 4 วิชาหลัก', '5', '3', 'ENT', '2010-03-01', '2010-03-31', 'Mon', '09:00:00', '15:30:00', '8500', '80', '0', '1324605643.gif', '1', '0');
INSERT INTO `course` VALUES ('80', 'BTS0080', 'คอร์ส English สำหรับสอบผู้ช่วยผู้พิพากษา อัยการ และผู้สนใจทั่วไป', 'อัยการEng', 'หลักสูตรภาษาอังกฤษสำหรับการเตรียมตัวสอบผู้ช่วยผู้พิากษา หรืออัยการผู้ช่วยขึ้น โดยในหลักสูตรนี้ ทุกท่านจะได้เรียนกับอาจารย์ผู้เชี่ยวชาญทางด้านภาษาอังกฤษสำหรับกฎหมาย (English for Lawyer) โดยตรงที่จะมาวิเคราะห์แนวข้อสอบจริงที่เคยสอบมาทั้งหมดให้ทุกท่านทราบ พร้อมทั้งสอนเทคนิคการทำข้อสอบในส่วนนี้ให้ได้คะแนนมากที่สุด เพื่อช่วยให้ท่านบรรลุเป้าหมายสอบเป็น ผู้พิพากษา หรือ อัยการ ได้สมตามความตั้งใจ\r\n', '5', '12', 'LAW', '2010-03-02', '2010-05-18', 'Mon', '18:00:00', '21:00:00', '15000', '35', '0', '1146568712.bmp', '1', '0');
INSERT INTO `course` VALUES ('81', 'BTS0081', 'คอร์ส Pre Law', 'Pre Law', 'หลักสูตรเตรียมความพร้อมสำหรับน้องๆที่กำลังจะเข้าศึกษาในระดับชั้นปีที่ 1 คณะนิติศาสตร์ ทุกสถาบันฯ ที่จะทำให้น้องๆมีโอกาสประสบความสำเร็จในการเรียน โดยเริ่มตั้งแต่วิชากฎหมายวิชาแรกที่จะเป็นพื้นฐานสำคัญในการเรียนกฎหมายต่อไป\r\n', '2', '14', 'ETC', '2010-02-26', '2010-03-25', 'Mon', '21:00:00', '23:30:00', '8000', '80', '0', '490547859.jpg', '1', '0');
INSERT INTO `course` VALUES ('82', 'BTS0082', 'คอร์สเรียนความถนัดทางวิทยาศาสตร์ ( PAT 2 )', 'PAT2', 'ในคอร์สนี้น้องๆจะได้เรียนสรุปเนื้อหา พร้อมทั้งสูตรที่จำเป็น และเรียนรู้เทคนิคในการทำโจทย์ทั้งหมดทุกวิชาที่ใช้ในการสอบจริง (ฟิสิกส์ เคมี ชีวะ และเนื้อหาอื่นๆนอกเหนือจาก 3 วิชาหลักนี้) จากครูผู้สอนที่จบมาทางด้านนี้โดยตรง และในคอร์สจะเน้นการตะลุยโจทย์ตามแนวข้อสอบครั้งล่าสุดที่ผ่านมา เพื่อให้น้องๆสามารถนำไปใช้ในการทำข้อสอบจริงได้อย่างถูกต้อง แม่นยำ และรวดเร็ว', '4', '6', 'ETC', '2009-11-07', '2010-02-20', 'Mon', '13:30:00', '17:30:00', '5000', '120', '0', '2079572006.gif', '1', '0');
INSERT INTO `course` VALUES ('83', 'BTS0083', 'คอร์ส Intensive O-Net \'53 (ตรงแนวข้อสอบใหม่)', 'IntONet53', 'เตรียมพร้อมรับมือกับข้อสอบ O-Net แนวใหม่ กระดาษคำตอบแบบใหม่... ที่กำลังเป็นที่วิตกกังวลของน้อง ๆ ม. 6 ที่ต้องสอบในปีนี้ (คลิ๊กดูกระดาษคำตอบแบบใหม่ที่นี้) บางวิชาก็ต้องตอบถูก 2 คำตอบ... ถึงจะได้คะแนน หรือ บางวิชาก็ตอบได้มากกว่า 1 คำตอบ... ตอบไม่ครบก็ไม่ได้คะแนน หรือ บางวิชาก็เป็นชุดคำตอบ... ต้องถูกทั้งชุดถึงจะได้คะแนน เป็นต้น\r\n     ที่ The BTS เรามีคำตอบทุกข้อสงสัยของน้อง ๆ ที่จะช่วยทำให้น้องสามารถทำข้อสอบ O-Net ได้มากขึ้น กับ คอร์ส Intensive O-Net ที่เราได้เชิญอาจารย์ผู้เชี่ยวชาญและมีประสบการณ์มาไขข้อข้องใจ และเก็งข้อสอบแบบเน้น ๆ ให้น้อง ๆ ตรงตามข้อสอบแนวใหม่!!! ก่อนใคร ', '5', '3', 'MTH', '2010-02-21', '2010-06-22', 'Mon', '06:00:00', '13:00:00', '7500', '50', '0', '1950519865.gif', '1', '0');
INSERT INTO `course` VALUES ('84', 'BTS0084', 'กวดเข้มคณิตศาสตร์เพื่อการสอบชิงทุนรัฐบาล', 'คณิตชิงทุนรัฐ', 'สำหรับคอร์สนี้น้องๆจะได้เรียนกับสุดยอดครูคณิตศาสตร์ระดับประเทศตัวจริง ที่มีประสบการณ์ติวเข้มให้กับนักเรียนเก่งๆระดับหัวกะทิของประเทศมาแล้วหลายต่อหลายคนมานานนับสิบปี โดยน้องๆจะได้รับการการติวเข้ม แบบวิเคราะห์ เจาะลึก ตามแนวข้อสอบจริงที่เคยออกมาทั้งหมด ครบถ้วนทุกเนื้อหา พร้อมทั้งการเก็งแนวข้อสอบที่คาดว่าจะออกในปีนี้อีกด้วย ทั้งนี้ตั้งใจรับนักเรียนในคอร์สนี้จำกัดเพียง 30 คนเท่านั้น เพราะอาจารย์ต้องการดูแลน้องๆทุกคนอย่างทั่วถึง เป็นกันเอง โดยน้องๆทุกคนจะได้มีโอกาสฝึกทำโจทย์แนวคิดวิเคราะห์อย่างจริงจัง และหากมีข้อสงสัยก็สามารถซักถามจากอาจารย์ได้ตลอดเวลา ซึ่งจะทำให้น้องๆทุกคนมีความพร้อมก่อนที่จะเข้าสู่สนามสอบจริงมากที่สุด', '2', '127', 'LAW', '2010-02-21', '2010-05-13', 'Mon', '06:00:00', '06:00:00', '10000', '50', '0', '400810762.jpg', '1', '0');

-- ----------------------------
-- Table structure for `paid_group`
-- ----------------------------
DROP TABLE IF EXISTS `paid_group`;
CREATE TABLE `paid_group` (
  `paid_group_id` int(10) unsigned NOT NULL,
  `name` varchar(100) NOT NULL DEFAULT '' COMMENT 'ex: ;100000:10;500000;20;1000000:30;',
  `rate_info` varchar(300) NOT NULL DEFAULT '' COMMENT 'ex: ;100000:10;500000;20;1000000:30;',
  `current_round` int(10) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`paid_group_id`),
  KEY `promotiom_id` (`paid_group_id`),
  KEY `course_id` (`rate_info`)
) ENGINE=InnoDB DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of paid_group
-- ----------------------------
INSERT INTO `paid_group` VALUES ('1', '111', '0:5;100000:10;300000:20;700000:30', '1');
INSERT INTO `paid_group` VALUES ('2', '222', '0:5;200000:10;400000:20;600000:30', '1');
INSERT INTO `paid_group` VALUES ('3', '333', '0:5;100000:10;300000:20;700000:30', '1');
INSERT INTO `paid_group` VALUES ('4', '444', '0:5;100000:10;300000:20;700000:30', '1');
INSERT INTO `paid_group` VALUES ('5', '555', '0:5;100000:10;300000:20;700000:30', '1');
INSERT INTO `paid_group` VALUES ('6', 'NullGroup', '0:5;100000:10;300000:20;700000:30', '1');

-- ----------------------------
-- Table structure for `payment`
-- ----------------------------
DROP TABLE IF EXISTS `payment`;
CREATE TABLE `payment` (
  `course_id` int(10) unsigned NOT NULL DEFAULT '0',
  `last_paid_date` datetime NOT NULL,
  `sum_all_cost` int(10) unsigned NOT NULL COMMENT 'equal cost if no promotion',
  `sum_max_payable` int(10) unsigned NOT NULL COMMENT 'equal cost if no promotion',
  `sum_paid_cost` int(10) unsigned NOT NULL COMMENT 'equal cost if no promotion',
  `status` int(10) unsigned NOT NULL DEFAULT '0' COMMENT '0=valid 1=cancelled',
  `paid_round` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'round to calculate accumulate income. will be increase once reset income.',
  PRIMARY KEY (`course_id`),
  KEY `course_id` (`course_id`)
) ENGINE=InnoDB DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of payment
-- ----------------------------
INSERT INTO `payment` VALUES ('1', '2010-03-27 03:04:16', '757019', '122102', '324400', '0', '1');
INSERT INTO `payment` VALUES ('2', '2010-06-14 00:59:34', '100167', '5016', '0', '0', '1');
INSERT INTO `payment` VALUES ('3', '2010-06-10 00:23:26', '139000', '6950', '0', '0', '1');
INSERT INTO `payment` VALUES ('80', '2010-03-30 16:14:18', '323210', '29641', '77907', '0', '1');
INSERT INTO `payment` VALUES ('81', '2010-06-13 23:48:23', '98790', '4939', '0', '0', '1');
INSERT INTO `payment` VALUES ('82', '2010-03-30 13:48:47', '50247', '2512', '30641', '0', '1');
INSERT INTO `payment` VALUES ('83', '2010-06-13 23:48:23', '172371', '8618', '0', '0', '1');
INSERT INTO `payment` VALUES ('84', '2010-03-30 16:47:01', '250500', '20050', '33000', '0', '1');

-- ----------------------------
-- Table structure for `payment_history`
-- ----------------------------
DROP TABLE IF EXISTS `payment_history`;
CREATE TABLE `payment_history` (
  `payment_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `course_id` int(10) unsigned NOT NULL DEFAULT '0',
  `paid_date` datetime NOT NULL,
  `paid_cost` int(10) unsigned NOT NULL COMMENT 'equal cost if no promotion',
  `sum_all_cost` int(10) unsigned NOT NULL COMMENT 'equal cost if no promotion',
  `sum_max_payable` int(10) unsigned NOT NULL COMMENT 'equal cost if no promotion',
  `sum_paid_cost` int(10) unsigned NOT NULL COMMENT 'equal cost if no promotion',
  `cost_info` varchar(100) NOT NULL,
  `branch_id` int(10) unsigned NOT NULL COMMENT 'transaction at which branch',
  `paid_round` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'round to calculate accumulate income. will be increase once reset income.',
  `receiver_teacher_id` int(10) NOT NULL DEFAULT '0',
  `username` varchar(50) NOT NULL,
  PRIMARY KEY (`payment_id`),
  KEY `course_id` (`payment_id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of payment_history
-- ----------------------------
INSERT INTO `payment_history` VALUES ('1', '1', '2010-03-01 00:00:00', '100000', '150000', '0', '0', '0:5;100000:10;300000:20;700000:30', '1', '0', '1', 'netta');
INSERT INTO `payment_history` VALUES ('2', '1', '2010-03-12 01:56:25', '50000', '250000', '0', '100000', '0:5;100000:10;300000:20;700000:30', '1', '0', '2', 'kizze');
INSERT INTO `payment_history` VALUES ('3', '1', '2010-03-22 01:57:14', '50000', '280000', '0', '150000', '0:5;100000:10;300000:20;700000:30', '1', '0', '1', 'netta');
INSERT INTO `payment_history` VALUES ('4', '1', '2010-03-26 00:00:00', '10000', '331316', '0', '200000', '0:5;100000:10;300000:20;700000:30', '1', '1', '2', 'netta');
INSERT INTO `payment_history` VALUES ('5', '1', '2010-03-26 00:00:00', '50000', '331316', '0', '210000', '0:5;100000:10;300000:20;700000:30', '1', '1', '13', 'netta');
INSERT INTO `payment_history` VALUES ('6', '1', '2010-03-26 00:00:00', '20000', '331316', '0', '260000', '0:5;100000:10;300000:20;700000:30', '1', '1', '1', 'netta');
INSERT INTO `payment_history` VALUES ('7', '1', '2010-03-26 02:32:08', '1000', '331316', '0', '280000', '0:5;100000:10;300000:20;700000:30', '1', '1', '1', 'netta');
INSERT INTO `payment_history` VALUES ('8', '1', '2010-03-27 02:19:57', '1000', '331316', '0', '281000', '0:5;100000:10;300000:20;700000:30', '1', '1', '1', 'netta');
INSERT INTO `payment_history` VALUES ('9', '1', '2010-03-27 02:21:11', '1000', '331316', '0', '282000', '0:5;100000:10;300000:20;700000:30', '1', '1', '1', 'netta');
INSERT INTO `payment_history` VALUES ('10', '1', '2010-03-27 02:22:25', '1000', '331316', '0', '283000', '0:5;100000:10;300000:20;700000:30', '1', '1', '2', 'netta');
INSERT INTO `payment_history` VALUES ('11', '1', '2010-03-27 02:24:34', '5000', '331316', '0', '284000', '0:5;100000:10;300000:20;700000:30', '1', '1', '2', 'netta');
INSERT INTO `payment_history` VALUES ('12', '1', '2010-03-27 02:31:49', '100', '331316', '0', '289000', '0:5;100000:10;300000:20;700000:30', '1', '1', '1', 'netta');
INSERT INTO `payment_history` VALUES ('13', '1', '2010-03-27 02:35:08', '100', '331316', '0', '289100', '0:5;100000:10;300000:20;700000:30', '1', '1', '2', 'netta');
INSERT INTO `payment_history` VALUES ('14', '1', '2010-03-27 02:36:15', '20000', '331316', '0', '289200', '0:5;100000:10;300000:20;700000:30', '1', '1', '2', 'netta');
INSERT INTO `payment_history` VALUES ('15', '1', '2010-03-27 02:39:36', '15000', '731316', '0', '309200', '0:5;100000:10;300000:20;700000:30', '1', '1', '1', 'netta');
INSERT INTO `payment_history` VALUES ('16', '1', '2010-03-27 03:02:27', '100', '731316', '0', '324200', '0:5;100000:10;300000:20;700000:30', '1', '1', '1', 'netta');
INSERT INTO `payment_history` VALUES ('17', '1', '2010-03-27 03:04:16', '100', '731316', '0', '324300', '0:5;100000:10;300000:20;700000:30', '1', '1', '1', 'netta');
INSERT INTO `payment_history` VALUES ('18', '82', '2010-03-30 13:48:47', '30641', '30641', '0', '0', '0:5;100000:10;300000:20;700000:30', '1', '1', '6', 'netta');
INSERT INTO `payment_history` VALUES ('19', '80', '2010-03-30 16:14:18', '77907', '259690', '77907', '0', '0:5;100000:10;300000:20;700000:30', '1', '1', '12', 'netta');
INSERT INTO `payment_history` VALUES ('20', '84', '2010-03-30 16:35:03', '20000', '210500', '63150', '0', '0:5;100000:10;300000:20;700000:30', '1', '1', '2', 'netta');
INSERT INTO `payment_history` VALUES ('21', '84', '2010-03-30 16:45:25', '5000', '210500', '63150', '20000', '0:5;100000:10;300000:20;700000:30', '1', '1', '1', 'netta');
INSERT INTO `payment_history` VALUES ('22', '84', '2010-03-30 16:47:01', '8000', '210500', '63150', '25000', '0:5;100000:10;300000:20;700000:30', '1', '1', '1', 'netta');

-- ----------------------------
-- Table structure for `promotion`
-- ----------------------------
DROP TABLE IF EXISTS `promotion`;
CREATE TABLE `promotion` (
  `promotion_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `promotion_name` varchar(100) NOT NULL,
  `promotion_desc` varchar(1000) NOT NULL,
  `cost` int(10) unsigned NOT NULL DEFAULT '0',
  `is_active` int(4) NOT NULL DEFAULT '1',
  `is_paid` int(11) NOT NULL DEFAULT '0' COMMENT '0=notpaid 1=paid',
  PRIMARY KEY (`promotion_id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of promotion
-- ----------------------------
INSERT INTO `promotion` VALUES ('1', 'นักกฏหมาย COMBO', 'สำหรับนักกฏหมายมือฉมัง', '5000', '1', '0');
INSERT INTO `promotion` VALUES ('2', 'ม.ปลาย PLUS', 'เสริมสร้างความรู้ม.ปลาย สำหรับเตรียมเอ็น', '1000', '1', '0');
INSERT INTO `promotion` VALUES ('3', 'เตรียมเอ็น MAX ', 'เตรียมเอ็น  \"ติดชัวร์\"', '27000', '1', '0');
INSERT INTO `promotion` VALUES ('4', 'asdasdasd', 'asdasdasd', '5000', '0', '0');
INSERT INTO `promotion` VALUES ('5', 'ughgfhfgh', 'vcvchgdhgf', '14000', '1', '0');
INSERT INTO `promotion` VALUES ('6', 'xxxxxxx', 'xxxx', '1', '1', '0');
INSERT INTO `promotion` VALUES ('7', 'yyyyyyy', 'yyyyy', '8000', '1', '0');
INSERT INTO `promotion` VALUES ('8', 'zzzzz', 'zzzzz', '5000', '1', '0');
INSERT INTO `promotion` VALUES ('9', 'error', '', '8000', '0', '0');

-- ----------------------------
-- Table structure for `promotion_course_mapping`
-- ----------------------------
DROP TABLE IF EXISTS `promotion_course_mapping`;
CREATE TABLE `promotion_course_mapping` (
  `promotion_id` int(10) unsigned NOT NULL,
  `course_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`promotion_id`,`course_id`),
  KEY `promotiom_id` (`promotion_id`),
  KEY `course_id` (`course_id`)
) ENGINE=InnoDB DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of promotion_course_mapping
-- ----------------------------
INSERT INTO `promotion_course_mapping` VALUES ('1', '1');
INSERT INTO `promotion_course_mapping` VALUES ('1', '2');
INSERT INTO `promotion_course_mapping` VALUES ('2', '1');
INSERT INTO `promotion_course_mapping` VALUES ('2', '82');
INSERT INTO `promotion_course_mapping` VALUES ('2', '83');
INSERT INTO `promotion_course_mapping` VALUES ('3', '2');
INSERT INTO `promotion_course_mapping` VALUES ('3', '3');
INSERT INTO `promotion_course_mapping` VALUES ('3', '82');
INSERT INTO `promotion_course_mapping` VALUES ('3', '83');
INSERT INTO `promotion_course_mapping` VALUES ('3', '84');
INSERT INTO `promotion_course_mapping` VALUES ('4', '1');
INSERT INTO `promotion_course_mapping` VALUES ('4', '2');
INSERT INTO `promotion_course_mapping` VALUES ('4', '80');
INSERT INTO `promotion_course_mapping` VALUES ('4', '82');
INSERT INTO `promotion_course_mapping` VALUES ('4', '83');
INSERT INTO `promotion_course_mapping` VALUES ('5', '80');
INSERT INTO `promotion_course_mapping` VALUES ('5', '81');
INSERT INTO `promotion_course_mapping` VALUES ('6', '81');
INSERT INTO `promotion_course_mapping` VALUES ('6', '82');
INSERT INTO `promotion_course_mapping` VALUES ('7', '80');
INSERT INTO `promotion_course_mapping` VALUES ('7', '82');
INSERT INTO `promotion_course_mapping` VALUES ('8', '82');
INSERT INTO `promotion_course_mapping` VALUES ('8', '83');
INSERT INTO `promotion_course_mapping` VALUES ('9', '3');

-- ----------------------------
-- Table structure for `registration`
-- ----------------------------
DROP TABLE IF EXISTS `registration`;
CREATE TABLE `registration` (
  `regis_id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'uniqe per course/pro - student',
  `transaction_id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'one transaction consists of multiple register_id',
  `regis_date` datetime NOT NULL,
  `student_id` int(10) unsigned NOT NULL,
  `course_id` int(10) unsigned NOT NULL DEFAULT '0',
  `discounted_cost` int(10) unsigned NOT NULL COMMENT 'cost after discount. will be devided by ratio if registered by promotion',
  `promotion_id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT '=0 if register by course. no promotion',
  `full_cost` int(10) unsigned NOT NULL COMMENT 'equal cost if no promotion',
  `seat_no` varchar(20) DEFAULT '',
  `branch_id` int(10) unsigned NOT NULL COMMENT 'transaction at which branch',
  `paid_method` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Cash=0, Transfer=1, Credit=2, SCB=3',
  `paid_round` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'round to calculate accumulate income. will be increase once reset income.',
  `username` varchar(50) NOT NULL DEFAULT '',
  `is_paid` int(10) unsigned NOT NULL DEFAULT '0',
  `status` int(10) unsigned NOT NULL DEFAULT '0' COMMENT '0=valid 1=cancelled',
  PRIMARY KEY (`regis_id`),
  KEY `promotiom_id` (`student_id`),
  KEY `course_id` (`course_id`)
) ENGINE=InnoDB AUTO_INCREMENT=255 DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of registration
-- ----------------------------
INSERT INTO `registration` VALUES ('40', '2', '2010-03-18 00:00:00', '2', '1', '2222', '1', '2222', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('41', '2', '2010-03-18 00:00:00', '2', '2', '2777', '1', '2777', '', '2', '0', '0', 'front', '1', '1');
INSERT INTO `registration` VALUES ('42', '2', '2010-03-18 00:00:00', '2', '81', '0', '6', '0', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('43', '2', '2010-03-18 00:00:00', '2', '82', '0', '6', '0', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('44', '2', '2010-03-18 00:00:00', '2', '80', '15000', '0', '15000', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('45', '2', '2010-03-18 00:00:00', '2', '84', '10000', '0', '10000', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('46', '2', '2010-03-21 00:00:00', '80', '1', '2222', '1', '2222', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('47', '3', '2010-03-21 00:00:00', '80', '2', '2777', '1', '2777', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('48', '3', '2010-03-21 00:00:00', '80', '80', '6000', '7', '6000', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('49', '3', '2010-03-21 00:00:00', '80', '82', '2000', '7', '2000', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('50', '3', '2010-03-21 00:00:00', '80', '84', '10000', '0', '10000', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('51', '4', '2010-03-21 00:00:00', '2', '1', '2222', '1', '2222', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('52', '4', '2010-03-21 00:00:00', '2', '2', '2778', '1', '2778', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('53', '4', '2010-03-21 00:00:00', '2', '82', '5000', '0', '5000', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('54', '5', '2010-03-21 00:00:00', '3', '1', '2222', '1', '2222', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('55', '5', '2010-03-21 00:00:00', '3', '2', '2778', '1', '2778', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('56', '5', '2010-03-21 00:00:00', '3', '80', '15000', '0', '15000', '333', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('57', '6', '2010-03-21 00:00:00', '81', '1', '2222', '1', '2222', '111', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('58', '6', '2010-03-21 00:00:00', '81', '2', '2778', '1', '2778', '222', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('59', '6', '2010-03-21 00:00:00', '81', '80', '15000', '0', '15000', '333', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('60', '7', '2010-03-21 00:00:00', '3', '1', '0', '2', '242', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('61', '7', '2010-03-21 00:00:00', '3', '82', '0', '2', '303', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('62', '7', '2010-03-21 00:00:00', '3', '83', '1', '2', '455', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('63', '7', '2010-03-21 00:00:00', '3', '84', '10000', '0', '10000', '', '2', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('64', '8', '2010-03-21 00:00:00', '4', '80', '6000', '7', '6000', '555', '1', '2', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('65', '8', '2010-03-21 00:00:00', '4', '82', '2000', '7', '2000', '666', '1', '2', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('66', '8', '2010-03-21 00:00:00', '4', '1', '4000', '0', '4000', '444', '1', '2', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('67', '9', '2010-03-22 00:00:00', '5', '1', '2222', '1', '2222', '22', '1', '1', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('68', '9', '2010-03-22 00:00:00', '5', '2', '2778', '1', '2778', '11', '1', '1', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('69', '10', '2010-03-22 00:00:00', '81', '82', '5000', '0', '5000', '', '1', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('70', '10', '2010-03-22 00:00:00', '81', '84', '10000', '0', '10000', '', '1', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('71', '11', '2010-03-22 01:04:39', '10', '80', '15000', '0', '15000', '', '1', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('72', '11', '2010-03-22 01:04:39', '10', '83', '7500', '0', '7500', '', '1', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('73', '12', '2010-03-22 01:45:46', '1', '1', '4000', '0', '4000', '', '1', '0', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('74', '13', '2010-03-22 01:51:31', '1', '1', '2500', '0', '4000', '', '1', '2', '0', 'front', '1', '0');
INSERT INTO `registration` VALUES ('75', '14', '2010-03-22 02:00:34', '4', '1', '4000', '0', '4000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('76', '15', '2010-03-22 02:11:03', '1', '1', '242', '2', '242', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('77', '15', '2010-03-22 02:11:03', '1', '82', '303', '2', '303', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('78', '15', '2010-03-22 02:11:03', '1', '83', '455', '2', '455', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('79', '15', '2010-03-22 02:11:03', '1', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('80', '15', '2010-03-22 02:11:03', '1', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('81', '15', '2010-03-22 02:11:03', '1', '2', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('82', '15', '2010-03-22 02:11:03', '1', '3', '8500', '0', '8500', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('83', '15', '2010-03-22 02:11:03', '1', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('84', '16', '2010-03-22 02:23:16', '2', '1', '242', '2', '242', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('85', '16', '2010-03-22 02:23:16', '2', '82', '303', '2', '303', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('86', '16', '2010-03-22 02:23:16', '2', '83', '455', '2', '455', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('87', '16', '2010-03-22 02:23:16', '2', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('88', '16', '2010-03-22 02:23:16', '2', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('89', '16', '2010-03-22 02:23:16', '2', '2', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('90', '16', '2010-03-22 02:23:16', '2', '3', '5000', '0', '8500', '', '1', '0', '0', 'netta', '0', '1');
INSERT INTO `registration` VALUES ('91', '16', '2010-03-22 02:23:16', '2', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('92', '17', '2010-03-25 02:56:13', '6', '80', '6000', '7', '6000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('93', '17', '2010-03-25 02:56:13', '6', '82', '2000', '7', '2000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('94', '17', '2010-03-25 02:56:13', '6', '1', '3000', '0', '4000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('95', '18', '2010-03-26 02:19:00', '5', '1', '250000', '0', '4000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('96', '19', '2010-03-26 02:19:28', '1', '1', '50000', '0', '4000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('97', '20', '2010-03-27 02:38:19', '2', '1', '400000', '0', '4000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('100', '21', '2010-03-28 19:13:46', '83', '1', '4000', '0', '4000', 'A021', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('101', '21', '2010-03-28 19:13:46', '83', '84', '8000', '0', '10000', 'B013', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('102', '21', '2010-03-28 19:13:46', '83', '83', '7500', '0', '7500', 'C014', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('103', '22', '2010-03-28 19:15:06', '81', '81', '0', '6', '0', '', '1', '2', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('104', '22', '2010-03-28 19:15:06', '81', '82', '1', '6', '1', '', '1', '2', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('105', '22', '2010-03-28 19:15:06', '81', '80', '12000', '0', '15000', '', '1', '2', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('106', '23', '2010-03-28 23:43:18', '83', '82', '2000', '8', '2000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('107', '23', '2010-03-28 23:43:18', '83', '83', '3000', '8', '3000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('108', '23', '2010-03-28 23:43:18', '83', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('109', '24', '2010-03-28 23:44:03', '83', '82', '2000', '8', '2000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('110', '24', '2010-03-28 23:44:03', '83', '83', '3000', '8', '3000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('111', '24', '2010-03-28 23:44:03', '83', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('112', '25', '2010-03-28 23:45:48', '1', '82', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('113', '26', '2010-03-28 23:48:45', '81', '81', '0', '6', '0', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('114', '26', '2010-03-28 23:48:45', '81', '82', '1', '6', '1', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('115', '26', '2010-03-28 23:48:45', '81', '84', '8000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('116', '26', '2010-03-28 23:48:45', '81', '83', '7500', '0', '7500', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('117', '27', '2010-03-28 23:51:28', '81', '1', '242', '2', '242', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('118', '27', '2010-03-28 23:51:28', '81', '82', '303', '2', '303', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('119', '27', '2010-03-28 23:51:28', '81', '83', '455', '2', '455', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('120', '27', '2010-03-28 23:51:28', '81', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('121', '27', '2010-03-28 23:51:28', '81', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('122', '27', '2010-03-28 23:51:28', '81', '2', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('123', '27', '2010-03-28 23:51:28', '81', '84', '3000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('124', '28', '2010-03-28 23:56:54', '2', '1', '4000', '0', '4000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('125', '29', '2010-03-28 23:57:47', '83', '1', '242', '2', '242', '', '1', '2', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('126', '29', '2010-03-28 23:57:47', '83', '82', '303', '2', '303', '', '1', '2', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('127', '29', '2010-03-28 23:57:47', '83', '83', '455', '2', '455', '', '1', '2', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('128', '29', '2010-03-28 23:57:47', '83', '80', '9130', '5', '9130', '', '1', '2', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('129', '29', '2010-03-28 23:57:47', '83', '81', '4870', '5', '4870', '', '1', '2', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('130', '29', '2010-03-28 23:57:47', '83', '2', '5000', '0', '5000', '', '1', '2', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('131', '29', '2010-03-28 23:57:47', '83', '84', '7500', '0', '10000', '', '1', '2', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('132', '30', '2010-03-29 00:08:24', '3', '1', '242', '2', '242', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('133', '30', '2010-03-29 00:08:24', '3', '82', '303', '2', '303', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('134', '30', '2010-03-29 00:08:24', '3', '83', '455', '2', '455', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('135', '30', '2010-03-29 00:08:24', '3', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('136', '30', '2010-03-29 00:08:24', '3', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('137', '30', '2010-03-29 00:08:24', '3', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('138', '31', '2010-03-29 00:11:54', '83', '1', '242', '2', '242', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('139', '31', '2010-03-29 00:11:54', '83', '82', '303', '2', '303', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('140', '31', '2010-03-29 00:11:54', '83', '83', '455', '2', '455', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('141', '31', '2010-03-29 00:11:54', '83', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('142', '31', '2010-03-29 00:11:54', '83', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('143', '31', '2010-03-29 00:11:54', '83', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('144', '32', '2010-03-29 00:14:09', '1', '1', '242', '2', '242', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('145', '32', '2010-03-29 00:14:09', '1', '82', '303', '2', '303', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('146', '32', '2010-03-29 00:14:09', '1', '83', '455', '2', '455', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('147', '32', '2010-03-29 00:14:09', '1', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('148', '32', '2010-03-29 00:14:09', '1', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('149', '32', '2010-03-29 00:14:09', '1', '2', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('150', '32', '2010-03-29 00:14:09', '1', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('151', '33', '2010-03-29 00:26:11', '82', '1', '242', '2', '242', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('152', '33', '2010-03-29 00:26:11', '82', '82', '303', '2', '303', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('153', '33', '2010-03-29 00:26:11', '82', '83', '455', '2', '455', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('154', '33', '2010-03-29 00:26:11', '82', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('155', '33', '2010-03-29 00:26:11', '82', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('156', '33', '2010-03-29 00:26:11', '82', '2', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('157', '33', '2010-03-29 00:26:11', '82', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('158', '34', '2010-03-29 00:27:46', '81', '1', '242', '2', '242', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('159', '34', '2010-03-29 00:27:46', '81', '82', '303', '2', '303', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('160', '34', '2010-03-29 00:27:46', '81', '83', '455', '2', '455', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('161', '34', '2010-03-29 00:27:46', '81', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('162', '34', '2010-03-29 00:27:46', '81', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('163', '34', '2010-03-29 00:27:46', '81', '2', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('164', '34', '2010-03-29 00:27:46', '81', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('165', '35', '2010-03-29 00:34:32', '83', '1', '242', '2', '242', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('166', '35', '2010-03-29 00:34:32', '83', '82', '303', '2', '303', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('167', '35', '2010-03-29 00:34:32', '83', '83', '455', '2', '455', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('168', '35', '2010-03-29 00:34:32', '83', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('169', '35', '2010-03-29 00:34:32', '83', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('170', '35', '2010-03-29 00:34:32', '83', '2', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('171', '35', '2010-03-29 00:34:32', '83', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('172', '36', '2010-03-29 00:35:24', '10', '1', '242', '2', '242', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('173', '36', '2010-03-29 00:35:24', '10', '82', '303', '2', '303', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('174', '36', '2010-03-29 00:35:24', '10', '83', '455', '2', '455', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('175', '36', '2010-03-29 00:35:24', '10', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('176', '36', '2010-03-29 00:35:24', '10', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('177', '36', '2010-03-29 00:35:24', '10', '2', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('178', '36', '2010-03-29 00:35:24', '10', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('179', '37', '2010-03-29 00:36:30', '2', '1', '242', '2', '242', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('180', '37', '2010-03-29 00:36:30', '2', '82', '303', '2', '303', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('181', '37', '2010-03-29 00:36:30', '2', '83', '455', '2', '455', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('182', '37', '2010-03-29 00:36:30', '2', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('183', '37', '2010-03-29 00:36:30', '2', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('184', '37', '2010-03-29 00:36:30', '2', '2', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('185', '37', '2010-03-29 00:36:30', '2', '84', '4000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('186', '38', '2010-03-29 00:43:38', '2', '1', '2222', '1', '2222', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('187', '38', '2010-03-29 00:43:38', '2', '2', '2778', '1', '2778', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('188', '39', '2010-03-29 00:44:28', '6', '1', '2222', '1', '2222', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('189', '39', '2010-03-29 00:44:28', '6', '2', '2778', '1', '2778', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('190', '39', '2010-03-29 00:44:28', '6', '80', '15000', '0', '15000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('191', '40', '2010-03-29 00:56:22', '7', '1', '2222', '1', '2222', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('192', '40', '2010-03-29 00:56:22', '7', '2', '2778', '1', '2778', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('193', '41', '2010-03-29 00:57:00', '10', '81', '0', '6', '0', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('194', '41', '2010-03-29 00:57:00', '10', '82', '1', '6', '1', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('195', '42', '2010-03-29 00:57:29', '4', '1', '4000', '0', '4000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('196', '42', '2010-03-29 00:57:29', '4', '83', '7500', '0', '7500', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('197', '42', '2010-03-29 00:57:29', '4', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('198', '43', '2010-03-29 00:58:35', '5', '81', '0', '6', '0', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('199', '43', '2010-03-29 00:58:35', '5', '82', '1', '6', '1', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('200', '43', '2010-03-29 00:58:35', '5', '80', '15000', '0', '15000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('201', '44', '2010-03-29 00:59:26', '6', '80', '6000', '7', '6000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('202', '44', '2010-03-29 00:59:26', '6', '82', '2000', '7', '2000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('203', '44', '2010-03-29 00:59:26', '6', '83', '7500', '0', '7500', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('204', '45', '2010-03-29 00:59:54', '3', '81', '0', '6', '0', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('205', '45', '2010-03-29 00:59:54', '3', '82', '1', '6', '1', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('206', '45', '2010-03-29 00:59:54', '3', '2', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('207', '45', '2010-03-29 00:59:54', '3', '80', '15000', '0', '15000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('208', '46', '2010-03-29 01:00:22', '2', '81', '8000', '0', '8000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('209', '46', '2010-03-29 01:00:22', '2', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('210', '47', '2010-03-30 15:18:20', '2', '1', '242', '2', '242', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('211', '47', '2010-03-30 15:18:20', '2', '82', '303', '2', '303', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('212', '47', '2010-03-30 15:18:20', '2', '83', '455', '2', '455', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('213', '47', '2010-03-30 15:18:20', '2', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('214', '47', '2010-03-30 15:18:20', '2', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('215', '47', '2010-03-30 15:18:20', '2', '2', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('216', '47', '2010-03-30 15:18:20', '2', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('217', '48', '2010-03-30 15:56:36', '82', '3', '8500', '0', '8500', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('218', '49', '2010-04-12 05:08:25', '8', '3', '100000', '0', '8500', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('219', '49', '2010-04-12 05:08:25', '8', '83', '100000', '0', '7500', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('220', '50', '2010-05-13 01:00:26', '2', '2', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('221', '50', '2010-05-13 01:00:26', '2', '82', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('222', '50', '2010-05-13 01:00:26', '2', '3', '8500', '0', '8500', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('223', '51', '2010-05-13 01:03:06', '2', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('224', '51', '2010-05-13 01:03:06', '2', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('225', '51', '2010-05-13 01:03:06', '2', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('226', '52', '2010-05-23 16:13:40', '2', '80', '6000', '7', '6000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('227', '52', '2010-05-23 16:13:40', '2', '82', '2000', '7', '2000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('228', '53', '2010-05-23 16:19:01', '2', '1', '2222', '1', '2222', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('229', '53', '2010-05-23 16:19:01', '2', '2', '2778', '1', '2778', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('230', '53', '2010-05-23 16:19:01', '2', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('231', '53', '2010-05-23 16:19:01', '2', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('232', '54', '2010-05-23 16:53:19', '3', '1', '242', '2', '242', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('233', '54', '2010-05-23 16:53:19', '3', '82', '303', '2', '303', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('234', '54', '2010-05-23 16:53:19', '3', '83', '455', '2', '455', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('235', '54', '2010-05-23 16:53:19', '3', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('236', '54', '2010-05-23 16:53:19', '3', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('237', '54', '2010-05-23 16:53:19', '3', '2', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('238', '54', '2010-05-23 16:53:19', '3', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('239', '55', '2010-06-10 00:23:26', '83', '3', '8500', '0', '8500', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('240', '55', '2010-06-10 00:23:26', '83', '80', '15000', '0', '15000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('241', '55', '2010-06-10 00:23:26', '83', '83', '7500', '0', '7500', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('242', '55', '2010-06-10 00:23:26', '83', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('243', '56', '2010-06-10 00:24:05', '87', '80', '6000', '7', '6000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('244', '56', '2010-06-10 00:24:05', '87', '82', '2000', '7', '2000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('245', '56', '2010-06-10 00:24:05', '87', '83', '7500', '0', '7500', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('246', '57', '2010-06-10 00:43:15', '90', '80', '9130', '5', '9130', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('247', '57', '2010-06-10 00:43:15', '90', '81', '4870', '5', '4870', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('248', '57', '2010-06-10 00:43:15', '90', '84', '10000', '0', '10000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('249', '58', '2010-06-13 23:48:23', '91', '81', '8000', '0', '8000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('250', '58', '2010-06-13 23:48:23', '91', '83', '7500', '0', '7500', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('251', '59', '2010-06-14 00:45:17', '9', '82', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('252', '60', '2010-06-14 00:59:34', '2', '1', '1911', '1', '2222', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('253', '60', '2010-06-14 00:59:34', '2', '2', '2389', '1', '2778', '', '1', '0', '0', 'netta', '1', '0');
INSERT INTO `registration` VALUES ('254', '60', '2010-06-14 00:59:34', '2', '82', '5000', '0', '5000', '', '1', '0', '0', 'netta', '1', '0');

-- ----------------------------
-- Table structure for `role`
-- ----------------------------
DROP TABLE IF EXISTS `role`;
CREATE TABLE `role` (
  `role_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`role_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of role
-- ----------------------------
INSERT INTO `role` VALUES ('1', 'Admin');
INSERT INTO `role` VALUES ('2', 'Management');
INSERT INTO `role` VALUES ('3', 'Front Staff');

-- ----------------------------
-- Table structure for `room`
-- ----------------------------
DROP TABLE IF EXISTS `room`;
CREATE TABLE `room` (
  `room_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `branch_id` int(10) unsigned NOT NULL,
  `seat_no` int(10) NOT NULL DEFAULT '40',
  `img` varchar(50) NOT NULL DEFAULT 'noimg.jpg',
  `description` varchar(200) NOT NULL DEFAULT '-',
  PRIMARY KEY (`room_id`),
  KEY `FK_room_brach_id` (`branch_id`),
  CONSTRAINT `FK_room_brach_id` FOREIGN KEY (`branch_id`) REFERENCES `branch` (`branch_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of room
-- ----------------------------
INSERT INTO `room` VALUES ('2', 'ห้องพฤกษา', '1', '40', '1896849874.jpg', 'มีเครื่องปรับอากาศ \r\nมีโปรเจคเตอร์\r\nมีคอมพิวเตอร์');
INSERT INTO `room` VALUES ('4', 'ห้องสกุณา', '1', '59', '2074126957.jpg', 'มีไวท์บอร์ด');
INSERT INTO `room` VALUES ('5', 'ห้องเด็กโข่ง', '2', '50', '1519687463.jpg', 'ไม้หน้าสาม');
INSERT INTO `room` VALUES ('6', 'ห้องฉายวีดีโอ', '2', '67', 'noimg.jpg', 'วีดีโอ \' กหด \' \\\r\n\\\r\n\'\r\n\\\'\r\n\'\'\r\n\\\\');
INSERT INTO `room` VALUES ('10', 'dsfdsf', '1', '55', 'noimg.jpg', '');

-- ----------------------------
-- Table structure for `student`
-- ----------------------------
DROP TABLE IF EXISTS `student`;
CREATE TABLE `student` (
  `student_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `firstname` varchar(50) NOT NULL,
  `surname` varchar(50) NOT NULL,
  `nickname` varchar(20) NOT NULL,
  `tel` varchar(30) NOT NULL,
  `email` varchar(50) NOT NULL,
  `school` varchar(50) DEFAULT NULL,
  `level` int(4) NOT NULL DEFAULT '1',
  `sex` varchar(6) NOT NULL,
  `birthday` date DEFAULT NULL,
  `addr` varchar(100) DEFAULT '''-''',
  `image` varchar(50) DEFAULT NULL,
  `create_date` date DEFAULT NULL,
  `is_active` int(4) NOT NULL DEFAULT '1',
  PRIMARY KEY (`student_id`)
) ENGINE=InnoDB AUTO_INCREMENT=92 DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of student
-- ----------------------------
INSERT INTO `student` VALUES ('1', 'MuiMui', 'Kung', 'Mui', '022203012', 'muimui@hotmail.com', 'aqqentmmulhhhfsvrzzo', '14', 'Female', '1991-03-07', 'aqqentmmulhhhfsvrzzoqxondwfvhdioilgmlmrbplrnxipohs', 'student5.jpg', '2002-03-08', '1');
INSERT INTO `student` VALUES ('2', 'อันนพ', 'ด้านได้อายอด', 'อั๋น', '022203012', 'muimui@hotmail.com', 'aqqentmmulhhhfsvrzzo', '14', 'Male', '1991-03-07', 'aqqentmmulhhhfsvrzzoqxondwfvhdioilgmlmrbplrnxipohs', 'student5.jpg', '2002-03-08', '1');
INSERT INTO `student` VALUES ('3', 'นพพล', 'บัวใหญ่', 'พี่เป้า', '022203012', 'muimui@hotmail.com', 'aqqentmmulhhhfsvrzzo', '14', 'Male', '1991-03-07', 'aqqentmmulhhhfsvrzzoqxondwfvhdioilgmlmrbplrnxipohs', 'student5.jpg', '2002-03-08', '1');
INSERT INTO `student` VALUES ('4', 'ป๋าเบิร์ด', 'มาทำไม', 'Mui', '022203012', 'muimui@hotmail.com', 'aqqentmmulhhhfsvrzzo', '14', 'Female', '1448-03-07', 'aqqentmmulhhhfsvrzzoqxondwfvhdioilgmlmrbplrnxipohs', 'student5.jpg', '2002-03-08', '1');
INSERT INTO `student` VALUES ('5', 'MuiMui', 'Kung', 'Mui', '022203012', 'muimui@hotmail.com', 'aqqentmmulhhhfsvrzzo', '14', 'Female', '1991-03-07', 'aqqentmmulhhhfsvrzzoqxondwfvhdioilgmlmrbplrnxipohs', 'student5.jpg', '2002-03-08', '1');
INSERT INTO `student` VALUES ('6', 'MuiMui', 'Kung', 'Mui', '022203012', 'muimui@hotmail.com', 'aqqentmmulhhhfsvrzzo', '14', 'Female', '1991-03-07', 'aqqentmmulhhhfsvrzzoqxondwfvhdioilgmlmrbplrnxipohs', 'student5.jpg', '2002-03-08', '1');
INSERT INTO `student` VALUES ('7', 'MuiMui', 'Kung', 'Mui', '022203012', 'muimui@hotmail.com', 'aqqentmmulhhhfsvrzzo', '14', 'Female', '1991-03-07', 'aqqentmmulhhhfsvrzzoqxondwfvhdioilgmlmrbplrnxipohs', 'student5.jpg', '2002-03-08', '1');
INSERT INTO `student` VALUES ('8', 'MuiMui', 'Kung', 'Mui', '022203012', 'muimui@hotmail.com', 'aqqentmmulhhhfsvrzzo', '14', 'Female', '1991-03-07', 'aqqentmmulhhhfsvrzzoqxondwfvhdioilgmlmrbplrnxipohs', 'student5.jpg', '2002-03-08', '1');
INSERT INTO `student` VALUES ('9', 'MuiMui', 'Kung', 'Mui', '022203012', 'muimui@hotmail.com', 'aqqentmmulhhhfsvrzzo', '14', 'Female', '1991-03-07', 'aqqentmmulhhhfsvrzzoqxondwfvhdioilgmlmrbplrnxipohs', 'student5.jpg', '2002-03-08', '1');
INSERT INTO `student` VALUES ('10', 'MuiMui', 'Kung', 'Mui', '022203012', 'muimui@hotmail.com', 'aqqentmmulhhhfsvrzzo', '14', 'Female', '1991-03-07', 'aqqentmmulhhhfsvrzzoqxondwfvhdioilgmlmrbplrnxipohs', 'student5.jpg', '2002-03-08', '1');
INSERT INTO `student` VALUES ('80', 'Kizze', 'Desperado', 'Pong', '0853304020', 'kizze@hot.com', 'อนุบาลหมาน่อย', '15', 'Male', '1982-10-15', '475/1', 'noimg.jpg', '2010-03-18', '1');
INSERT INTO `student` VALUES ('81', 'Kizze', 'Desperado', 'Pong', '0853304020', 'kizze@hot.com', 'อนุบาลหมาน่อย', '15', 'Male', '1982-10-15', '475/1', 'noimg.jpg', '2010-03-18', '1');
INSERT INTO `student` VALUES ('82', 'Kizze', 'Desperado', 'Pong', '0853304020', 'kizze@hot.com', 'อนุบาลหมาน่อย', '15', 'Male', '1982-10-15', '475/1', 'noimg.jpg', '2010-03-18', '1');
INSERT INTO `student` VALUES ('83', 'Kizze', 'Desperado', 'Pong', '0853304020', 'kizze@hot.com', 'อนุบาลหมาน่อย', '15', 'Male', '1982-10-15', '475/1', 'noimg.jpg', '2010-03-18', '1');
INSERT INTO `student` VALUES ('85', 'bb11', 'bb22', 'bb33', '', '', '', '1', 'Female', '2010-04-12', 'dsdsgdfgdfg', 'noimg.jpg', '2010-04-12', '1');
INSERT INTO `student` VALUES ('86', '33333', '33333', '', '', '', '', '1', 'Male', '2010-04-12', '', 'noimg.jpg', '2010-04-12', '1');
INSERT INTO `student` VALUES ('90', 'เด็กติด', 'facebook', '', '', '', '', '12', 'Male', '2010-06-10', '', '204962853.gif', '2010-06-10', '1');
INSERT INTO `student` VALUES ('91', '123', '456', '', '', '', '', '1', 'Male', '2010-06-13', '', 'noimg.jpg', '2010-06-13', '1');

-- ----------------------------
-- Table structure for `teacher`
-- ----------------------------
DROP TABLE IF EXISTS `teacher`;
CREATE TABLE `teacher` (
  `teacher_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `firstname` varchar(50) NOT NULL,
  `surname` varchar(50) NOT NULL,
  `tel` varchar(30) DEFAULT '',
  `email` varchar(100) DEFAULT '',
  `sex` varchar(6) NOT NULL,
  `birthday` date DEFAULT NULL,
  `addr` varchar(100) DEFAULT '''-''',
  `image` varchar(50) DEFAULT NULL,
  `subject` varchar(100) DEFAULT NULL,
  `is_active` int(4) NOT NULL DEFAULT '1',
  `paid_group_id` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`teacher_id`)
) ENGINE=InnoDB AUTO_INCREMENT=162 DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of teacher
-- ----------------------------
INSERT INTO `teacher` VALUES ('1', 'ครูแจ๋ว', 'เสียงดี', '0899956631', 'weerawat@gmail.comsxxx', 'Female', '1953-02-04', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'kluay.jpg', 'เสียงดี', '1', '1');
INSERT INTO `teacher` VALUES ('2', 'ครูระเบียบ', 'เรื่องมาก', '0842236454, 0814586698', '', 'Female', '1965-01-31', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'ha.jpg', 'กฏหมาย', '1', '1');
INSERT INTO `teacher` VALUES ('3', 'ครูอับดุล', 'ชอบบูชา', '0899956631', '', 'Male', '2010-01-21', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'arm.jpg', 'คณิตศาสตรื', '1', '2');
INSERT INTO `teacher` VALUES ('4', 'testt', 'naja', '0829619523', '', 'Male', '2010-02-01', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', '990987659.gif', 'naja', '1', '2');
INSERT INTO `teacher` VALUES ('6', 'ครูมาลี', 'มีวิชา', '0899956631', '', 'Female', '2010-02-03', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', '295030370.jpg', 'มีวิชา', '1', '4');
INSERT INTO `teacher` VALUES ('11', 'asdasdasd', '', '0864525661', '', 'Female', '2004-02-11', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'noimg.jpg', '', '1', '3');
INSERT INTO `teacher` VALUES ('12', 'ครูสมชาย', 'สายเสมอ', '0846997687', '', 'Male', '2010-02-17', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', '420640427.jpg', 'สายเสมอ', '1', '5');
INSERT INTO `teacher` VALUES ('13', 'test', '', '0848969226', '', 'Male', '1953-02-25', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'noimg.jpg', '', '1', '1');
INSERT INTO `teacher` VALUES ('14', 'นางมารร้าย', 'โหดมาก', '084569875', '', 'Female', '2010-02-09', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', '1965406101.gif', 'สปช.', '1', '3');
INSERT INTO `teacher` VALUES ('15', 'hstqelsoxq', 'hstqelsoxqawbwk', '0837197823', '', 'Female', '1940-06-12', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'hstqelsoxqawbwkqmmvmfgthldmnnc', '1', '6');
INSERT INTO `teacher` VALUES ('80', 'jqmdihaflny', 'jqmdihaflnyqqwwk', '0896567732', '', 'Female', '1954-05-26', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'jqmdihaflnyqqwwkmfujutctjweatf', '1', '6');
INSERT INTO `teacher` VALUES ('81', 'cavztgjgkin', 'cavztgjgkinpqngk', '0848969226', '', 'Male', '1958-04-14', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'cavztgjgkinpqngkyartaqnynmrwpz', '1', '6');
INSERT INTO `teacher` VALUES ('82', 'cavztgjgkin', 'cavztgjgkinpqngk', '0852810658', '', 'Male', '1998-10-22', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'cavztgjgkinpqngkyartaqnynmrwpz', '1', '6');
INSERT INTO `teacher` VALUES ('83', 'ykienjcugwo', 'ykienjcugwoiyitu', '0814918236', '', 'Female', '1968-08-02', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'ykienjcugwoiyitugiboahbezcodtb', '1', '6');
INSERT INTO `teacher` VALUES ('84', 'hcnptjguftj', 'hcnptjguftjuyryt', '0816808675', '', 'Male', '1900-11-15', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'hcnptjguftjuyrytzgnudftuokibey', '1', '6');
INSERT INTO `teacher` VALUES ('85', 'dnzunlzichk', 'dnzunlzichknhlld', '0896516146', '', 'Female', '1952-09-22', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'dnzunlzichknhlldhoxpdxhaabfjia', '1', '6');
INSERT INTO `teacher` VALUES ('86', 'dnzunlzichk', 'dnzunlzichknhlld', '0846997687', '', 'Female', '1916-11-13', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'dnzunlzichknhlldhoxpdxhaabfjia', '1', '6');
INSERT INTO `teacher` VALUES ('87', 'mfeftldjbee', 'mfeftldjbeeaguqd', '0893211201', '', 'Female', '1985-04-15', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'mfeftldjbeeaguqdzmjuhvzppizgsx', '1', '6');
INSERT INTO `teacher` VALUES ('88', 'vxjpzkijbcz', 'vxjpzkijbczmgcwd', '0842699153', '', 'Female', '1992-09-25', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'vxjpzkijbczmgcwdskuzktsfdqtedu', '1', '6');
INSERT INTO `teacher` VALUES ('89', 'rhvvtnbxxqa', 'rhvvtnbxxqafpxjm', '0838464247', '', 'Male', '1991-06-06', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'rhvvtnbxxqafpxjmaseuklflqhqmhw', '1', '6');
INSERT INTO `teacher` VALUES ('90', 'rhvvtnbxxqa', 'rhvvtnbxxqafpxjm', '0864525661', '', 'Male', '1994-06-18', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'rhvvtnbxxqafpxjmaseuklflqhqmhw', '1', '6');
INSERT INTO `teacher` VALUES ('91', 'wknltqyltbw', 'wknltqyltbwkxbbw', '0863610895', '', 'Female', '1921-12-10', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'wknltqyltbwkxbbwbxaunamgqfhrwv', '1', '6');
INSERT INTO `teacher` VALUES ('92', 'lejmyszbokm', 'lejmyszbokmcfmyf', '0845393014', '', 'Female', '1981-03-15', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'lejmyszbokmcfmyfvbhztokrglsuvr', '1', '6');
INSERT INTO `teacher` VALUES ('93', 'ebxcewyefdy', 'ebxcewyefdyzvbox', '0852097317', '', 'Female', '1910-01-24', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'ebxcewyefdyzvboxpkkedspyxqtdjn', '1', '6');
INSERT INTO `teacher` VALUES ('94', 'jeoseyvsapu', 'jeoseyvsapufdfgh', '0895982301', '', 'Female', '1900-02-11', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'jeoseyvsapufdfghqqgegivtyokiym', '1', '6');
INSERT INTO `teacher` VALUES ('95', 'coyoqxeuzjj', 'coyoqxeuzjjedwqg', '0829619523', '', 'Male', '2001-11-26', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'coyoqxeuzjjedwqgcmcomegycdxeug', '1', '6');
INSERT INTO `teacher` VALUES ('96', 'coyoqxeuzjj', 'coyoqxeuzjjedwqg', '0861689781', '', 'Male', '1903-03-10', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'coyoqxeuzjjedwqgcmcomegycdxeug', '1', '6');
INSERT INTO `teacher` VALUES ('97', 'yzktkawhwxk', 'yzktkawhwxkxlqdq', '0816876177', '', 'Male', '1973-04-11', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'yzktkawhwxkxlqdqkunjmvueouvmyi', '1', '6');
INSERT INTO `teacher` VALUES ('98', 'hrpeqabivvf', 'hrpeqabivvfklziq', '0839604863', '', 'Female', '1932-06-07', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'hrpeqabivvfklziqdsypptmudbojif', '1', '6');
INSERT INTO `teacher` VALUES ('99', 'hrpeqabivvf', 'hrpeqabivvfklziq', '0869006310', '', 'Male', '1940-06-09', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'hrpeqabivvfklziqdsypptmudbojif', '1', '6');
INSERT INTO `teacher` VALUES ('100', 'dbcjkcuwrig', 'dbcjkcuwrigctuvz', '0842468329', '', 'Male', '1959-04-25', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'dbcjkcuwrigctuvzlzjkplaapsmrmh', '1', '6');
INSERT INTO `teacher` VALUES ('101', 'dbcjkcuwrig', 'dbcjkcuwrigctuvz', '0889538125', '', 'Female', '1992-07-21', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'dbcjkcuwrigctuvzlzjkplaapsmrmh', '1', '6');
INSERT INTO `teacher` VALUES ('102', 'mtgupcywrgb', 'mtgupcywrgbptcbz', '0883448247', '', 'Male', '1958-07-11', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'mtgupcywrgbptcbzdxupsjspeafpxe', '1', '6');
INSERT INTO `teacher` VALUES ('103', 'vllfvccxqdv', 'vllfvccxqdvctlgz', '0840009273', '', 'Male', '1964-12-16', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'vllfvccxqdvctlgzwvfuwhleshzmib', '1', '6');
INSERT INTO `teacher` VALUES ('104', 'rwykpevlnrx', 'rwykpevlnrxubgti', '0811557104', '', 'Male', '1979-08-18', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'rwykpevlnrxubgtiedqpwzylfywumd', '1', '6');
INSERT INTO `teacher` VALUES ('105', 'rwykpevlnrx', 'rwykpevlnrxubgti', '0857381292', '', 'Female', '1935-05-18', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'rwykpevlnrxubgtiedqpwzylfywumd', '1', '6');
INSERT INTO `teacher` VALUES ('106', 'aocvvealmor', 'aocvvealmorhboyi', '0890587480', '', 'Female', '1913-05-19', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'aocvvealmorhboyixbbuzxratgqsxa', '1', '6');
INSERT INTO `teacher` VALUES ('107', 'wypaphszics', 'wypaphszicszjjls', '0828422572', '', 'Male', '1932-06-04', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'wypaphszicszjjlsfjmpzpfggxnaac', '1', '6');
INSERT INTO `teacher` VALUES ('108', 'fqulvgxaian', 'fqulvgxaianmjrqr', '0844339036', '', 'Female', '1954-07-19', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'fqulvgxaianmjrqryhxucnxwuehxlz', '1', '6');
INSERT INTO `teacher` VALUES ('109', 'pizwbgbbhxh', 'pizwbgbbhxhzjavr', '0835963932', '', 'Male', '1926-03-17', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'pizwbgbbhxhzjavrrfizflpljmbvww', '1', '6');
INSERT INTO `teacher` VALUES ('110', 'ktlbvjuodlj', 'ktlbvjuodljrrvib', '0869576311', '', 'Male', '1988-11-21', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'ktlbvjuodljrrvibzmtufcdrvdyday', '1', '6');
INSERT INTO `teacher` VALUES ('111', 'ulqmbiypdid', 'ulqmbiypdiderdob', '0890447627', '', 'Male', '1943-01-15', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'ulqmbiypdiderdobskezibvgkksalv', '1', '6');
INSERT INTO `teacher` VALUES ('112', 'ulqmbiypdid', 'ulqmbiypdiderdob', '0855848009', '', 'Female', '1926-09-21', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'ulqmbiypdiderdobskezibvgkksalv', '1', '6');
INSERT INTO `teacher` VALUES ('113', 'qvdrvlrdzwe', 'qvdrvlrdzwexzyak', '0893257466', '', 'Female', '1950-07-25', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'qvdrvlrdzwexzyakaspuisjnwbpiox', '1', '6');
INSERT INTO `teacher` VALUES ('114', 'eqzsbnssufv', 'eqzsbnssufvphkyt', '0894895490', '', 'Female', '1943-12-10', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'eqzsbnssufvphkyttwwzogiymhalou', '1', '6');
INSERT INTO `teacher` VALUES ('115', 'skvtgpuhpnl', 'skvtgpuhpnlhqvvc', '0869319302', '', 'Female', '1990-11-21', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'skvtgpuhpnlhqvvcnzdevugicnkooq', '1', '6');
INSERT INTO `teacher` VALUES ('116', 'ccadmoyhplg', 'ccadmoyhplgtqeac', '0822959505', '', 'Male', '1933-08-25', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'ccadmoyhplgtqeacgxojyszyrvemyn', '1', '6');
INSERT INTO `teacher` VALUES ('117', 'xnmjgrrvlyh', 'xnmjgrrvlyhmyznm', '0830298691', '', 'Female', '1919-08-25', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'xnmjgrrvlyhmyznmofzeyknedlbucp', '1', '6');
INSERT INTO `teacher` VALUES ('118', 'xnmjgrrvlyh', 'xnmjgrrvlyhmyznm', '0844273077', '', 'Male', '1998-09-06', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'xnmjgrrvlyhmyznmofzeyknedlbucp', '1', '6');
INSERT INTO `teacher` VALUES ('119', 'hfrumrwwlwb', 'hfrumrwwlwbzyhtm', '0850946349', '', 'Female', '1914-12-11', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'hfrumrwwlwbzyhtmhdkkbiftstvsnm', '1', '6');
INSERT INTO `teacher` VALUES ('120', 'cpezguojhkd', 'cpezguojhkdrgcgv', '0875286718', '', 'Female', '1967-06-22', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'cpezguojhkdrgcgvolufbztaekszro', '1', '6');
INSERT INTO `teacher` VALUES ('121', 'cpezguojhkd', 'cpezguojhkdrgcgv', '0846385544', '', 'Female', '1974-08-19', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'cpezguojhkdrgcgvolufbztaekszro', '1', '6');
INSERT INTO `teacher` VALUES ('122', 'mhikmttkghx', 'mhikmttkghxegllv', '0820544400', '', 'Male', '1936-05-22', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'mhikmttkghxegllvhjgkexlptrmxcl', '1', '6');
INSERT INTO `teacher` VALUES ('123', 'mhikmttkghx', 'mhikmttkghxegllv', '0810418809', '', 'Female', '1940-03-24', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'mhikmttkghxegllvhjgkexlptrmxcl', '1', '6');
INSERT INTO `teacher` VALUES ('124', 'vznustxlges', 'vznustxlgesrgtqv', '0870023681', '', 'Male', '1960-06-01', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'vznustxlgesrgtqvahrpivdehzfvni', '1', '6');
INSERT INTO `teacher` VALUES ('125', 'rkaamvqycst', 'rkaamvqycstjoode', '0836300329', '', 'Female', '2001-11-12', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'rkaamvqycstjoodeipckhnrluqdcqk', '1', '6');
INSERT INTO `teacher` VALUES ('126', 'rkaamvqycst', 'rkaamvqycstjoode', '0813748609', '', 'Male', '2004-06-17', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'rkaamvqycstjoodeipckhnrluqdcqk', '1', '6');
INSERT INTO `teacher` VALUES ('127', 'acfksvuzcqn', 'acfksvuzcqnwowie', '0884497258', '', 'Female', '1969-07-16', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'acfksvuzcqnwowiebmnpllkaixwabh', '1', '1');
INSERT INTO `teacher` VALUES ('128', 'wmrqmynnyep', 'wmrqmynnyepowrvo', '0855225426', '', 'Female', '1931-01-06', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'wmrqmynnyepowrvojuxkkdygvouifj', '1', '6');
INSERT INTO `teacher` VALUES ('129', 'wmrqmynnyep', 'wmrqmynnyepowrvo', '0829440958', '', 'Female', '1979-08-13', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'wmrqmynnyepowrvojuxkkdygvouifj', '1', '6');
INSERT INTO `teacher` VALUES ('130', 'fewarxrnxbj', 'fewarxrnxbjbwaao', '0858038377', '', 'Female', '1984-01-02', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'fewarxrnxbjbwaaocsjpobqvjwngqg', '1', '6');
INSERT INTO `teacher` VALUES ('131', 'pwblxxwoxye', 'pwblxxwoxyeowifn', '0849393934', '', 'Female', '2002-01-24', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'pwblxxwoxyeowifnvquurzilydhdbd', '1', '6');
INSERT INTO `teacher` VALUES ('132', 'pwblxxwoxye', 'pwblxxwoxyeowifn', '0836858002', '', 'Female', '1928-05-19', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'pwblxxwoxyeowifnvquurzilydhdbd', '1', '6');
INSERT INTO `teacher` VALUES ('133', 'khnqraoctmf', 'khnqraoctmfgedsx', '0860667161', '', 'Female', '2003-10-13', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'khnqraoctmfgedsxdyeprrwrkuelef', '1', '6');
INSERT INTO `teacher` VALUES ('134', 'khnqraoctmf', 'khnqraoctmfgedsx', '0839690613', '', 'Male', '1999-06-23', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'khnqraoctmfgedsxdyeprrwrkuelef', '1', '6');
INSERT INTO `teacher` VALUES ('135', 'uzsbxztdskz', 'uzsbxztdskztelyx', '0814110982', '', 'Female', '1990-10-09', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'uzsbxztdskztelyxwwquupogzcyjpc', '1', '6');
INSERT INTO `teacher` VALUES ('136', 'uzsbxztdskz', 'uzsbxztdskztelyx', '0829301328', '', 'Male', '1948-02-03', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'uzsbxztdskztelyxwwquupogzcyjpc', '1', '6');
INSERT INTO `teacher` VALUES ('137', 'pjfgrcmqpxb', 'pjfgrcmqpxbmmglg', '0892359744', '', 'Female', '1966-11-25', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'pjfgrcmqpxbmmglgdeapugcnltvqtf', '1', '6');
INSERT INTO `teacher` VALUES ('138', 'zbkrxcqrovv', 'zbkrxcqrovvzmpqg', '0898268640', '', 'Female', '1941-06-09', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'zbkrxcqrovvzmpqgwcmuxevcaapoec', '1', '6');
INSERT INTO `teacher` VALUES ('139', 'itocdbusosq', 'itocdbusosqlmxvg', '0860404665', '', 'Male', '1911-03-20', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'itocdbusosqlmxvgpzxzacnrpijmpz', '1', '6');
INSERT INTO `teacher` VALUES ('140', 'itocdbusosq', 'itocdbusosqlmxvg', '0898173896', '', 'Male', '1957-08-15', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'itocdbusosqlmxvgpzxzacnrpijmpz', '1', '6');
INSERT INTO `teacher` VALUES ('141', 'eebhxenfkgr', 'eebhxenfkgreusip', '0827848637', '', 'Male', '1999-03-03', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'eebhxenfkgreusipxhhuaubxbzgutb', '1', '6');
INSERT INTO `teacher` VALUES ('142', 'nwgsdergjdm', 'nwgsdergjdmruanp', '0851300512', '', 'Male', '1941-10-26', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'nwgsdergjdmruanpqftzestnqgarey', '1', '6');
INSERT INTO `teacher` VALUES ('143', 'jgsxxgkugrn', 'jgsxxgkugrnjcvaz', '0815046076', '', 'Male', '1964-01-07', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'jgsxxgkugrnjcvazyndudkhtcxxzha', '1', '6');
INSERT INTO `teacher` VALUES ('144', 'jgsxxgkugrn', 'jgsxxgkugrnjcvaz', '0893131576', '', 'Male', '1958-11-16', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'jgsxxgkugrnjcvazyndudkhtcxxzha', '1', '6');
INSERT INTO `teacher` VALUES ('145', 'syxidgpufph', 'syxidgpufphwcefz', '0819356688', '', 'Male', '1983-11-16', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'syxidgpufphwcefzrlozhizirerxsx', '1', '6');
INSERT INTO `teacher` VALUES ('146', 'syxidgpufph', 'syxidgpufphwcefz', '0858943853', '', 'Male', '1949-08-08', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'syxidgpufphwcefzrlozhizirerxsx', '1', '6');
INSERT INTO `teacher` VALUES ('147', 'crctjftvfmc', 'crctjftvfmcjcmly', '0818289444', '', 'Male', '1965-03-18', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'crctjftvfmcjcmlykjaekgsygmkudu', '1', '6');
INSERT INTO `teacher` VALUES ('148', 'crctjftvfmc', 'crctjftvfmcjcmly', '0826134592', '', 'Male', '1951-08-09', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'crctjftvfmcjcmlykjaekgsygmkudu', '1', '6');
INSERT INTO `teacher` VALUES ('149', 'xboydimjbad', 'xboydimjbadbkhyi', '0849547744', '', 'Female', '1979-06-21', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher3.jpg', 'xboydimjbadbkhyisrkzkygesdichw', '1', '6');
INSERT INTO `teacher` VALUES ('150', 'httjiiqjaxy', 'httjiiqjaxyokpdi', '0820381092', '', 'Female', '1983-01-08', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'httjiiqjaxyokpdikpwfnwythkbast', '1', '6');
INSERT INTO `teacher` VALUES ('151', 'cdgodljxxlz', 'cdgodljxxlzgskqr', '0873727423', '', 'Male', '1969-04-03', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'cdgodljxxlzgskqrswgannmatbzhvv', '1', '6');
INSERT INTO `teacher` VALUES ('152', 'cdgodljxxlz', 'cdgodljxxlzgskqr', '0829700146', '', 'Female', '1922-01-02', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'cdgodljxxlzgskqrswgannmatbzhvv', '1', '6');
INSERT INTO `teacher` VALUES ('153', 'mwlziknywit', 'mwlziknywittstvr', '0818810256', '', 'Female', '1947-06-09', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher4.jpg', 'mwlziknywittstvrlurfqmepijsfgs', '1', '6');
INSERT INTO `teacher` VALUES ('154', 'vopkoksyvgo', 'vopkoksyvgogsbar', '0898779787', '', 'Female', '1970-08-27', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'vopkoksyvgogsbaresdktkwewqmdrp', '1', '6');
INSERT INTO `teacher` VALUES ('155', 'vopkoksyvgo', 'vopkoksyvgogsbar', '0862600648', '', 'Male', '1950-01-07', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher1.jpg', 'vopkoksyvgogsbaresdktkwewqmdrp', '1', '6');
INSERT INTO `teacher` VALUES ('156', 'rycpimkmsup', 'rycpimkmsupyawna', '0871977172', '', 'Female', '1975-07-23', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'rycpimkmsupyawnamanftbkkjhjlvr', '1', '6');
INSERT INTO `teacher` VALUES ('157', 'rycpimkmsup', 'rycpimkmsupyawna', '0813665392', '', 'Male', '1936-10-19', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher2.jpg', 'rycpimkmsupyawnamanftbkkjhjlvr', '1', '6');
INSERT INTO `teacher` VALUES ('158', 'aqhaompnrrk', 'aqhaompnrrklafsa', '0898311767', '', 'Male', '1951-07-02', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'teacher5.jpg', 'aqhaompnrrklafsafyykwzdaxpdigo', '1', '6');
INSERT INTO `teacher` VALUES ('160', 'ทดสอบ', '', '', '', 'Male', '2010-02-12', '15/50 สุขุมวิท  พระขโนง กรุงเทพ 10500', 'noimg.jpg', '', '1', '6');
INSERT INTO `teacher` VALUES ('161', '1111', '2222', '', '', 'Male', '2010-04-12', '', 'noimg.jpg', '', '1', '1');

-- ----------------------------
-- Table structure for `test`
-- ----------------------------
DROP TABLE IF EXISTS `test`;
CREATE TABLE `test` (
  `firstname` varchar(50) CHARACTER SET utf8 DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of test
-- ----------------------------
INSERT INTO `test` VALUES ('ดเกดเก');

-- ----------------------------
-- Table structure for `user`
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `username` varchar(50) CHARACTER SET latin1 NOT NULL,
  `passwd` varchar(255) CHARACTER SET latin1 NOT NULL,
  `role_id` int(10) unsigned DEFAULT NULL,
  `firstname` varchar(50) DEFAULT NULL,
  `surname` varchar(50) DEFAULT NULL,
  `is_valid` int(4) unsigned NOT NULL DEFAULT '1',
  `branch_id` int(10) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`username`),
  KEY `FK_user_role_id` (`role_id`),
  CONSTRAINT `FK_user_role_id` FOREIGN KEY (`role_id`) REFERENCES `role` (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=tis620;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES ('front', 'cc03e747a6afbbcbf8be7668acfebee5', '3', 'Front', 'Staff', '1', '1');
INSERT INTO `user` VALUES ('front2', 'cc03e747a6afbbcbf8be7668acfebee5', '3', 'Front2', 'Staff2', '1', '2');
INSERT INTO `user` VALUES ('kizze', 'cc03e747a6afbbcbf8be7668acfebee5', '2', 'Kittipong', 'test123', '1', '1');
INSERT INTO `user` VALUES ('netta', 'cc03e747a6afbbcbf8be7668acfebee5', '1', 'Weerawat', 'Seetalalai', '1', '1');

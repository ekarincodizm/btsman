-- CREATE DATABASE  IF NOT EXISTS `bts` /*!40100 DEFAULT CHARACTER SET tis620 COLLATE tis620_bin */;
USE `bts`;
-- MySQL dump 10.13  Distrib 5.6.24, for Win64 (x86_64)
--
-- Host: localhost    Database: bts
-- ------------------------------------------------------
-- Server version	5.6.10-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `authorization`
--

DROP TABLE IF EXISTS `authorization`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `authorization` (
  `auth_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `role_id` int(10) unsigned NOT NULL,
  `rightname` varchar(100) NOT NULL,
  `action` varchar(50) NOT NULL,
  PRIMARY KEY (`auth_id`),
  KEY `IDX_UNQ_authorize_name` (`role_id`),
  CONSTRAINT `FK_auth_role_id` FOREIGN KEY (`role_id`) REFERENCES `role` (`role_id`)
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `authorization`
--

LOCK TABLES `authorization` WRITE;
/*!40000 ALTER TABLE `authorization` DISABLE KEYS */;
INSERT INTO `authorization` VALUES (1,1,'Administrator.aspx','*'),(2,1,'BranchManage.aspx','*'),(3,1,'CourseManage.aspx','*'),(4,1,'Default.aspx','*'),(5,1,'Home.aspx','*'),(6,1,'PaidGroupManage.aspx','*'),(7,1,'PaymentManage.aspx','*'),(8,1,'PromotionManage.aspx','*'),(9,1,'RegisterCourse.aspx','*'),(10,1,'RegistrationManage.aspx','*'),(11,1,'ReportDailyRegistration.aspx','*'),(12,1,'ReportRegis.aspx','*'),(13,1,'RoomManage.aspx','*'),(14,1,'StudentManage.aspx','*'),(15,1,'TeacherManage.aspx','*'),(16,2,'Administrator.aspx','default'),(17,2,'Administrator.aspx','list'),(18,2,'BranchManage.aspx','default'),(19,2,'BranchManage.aspx','list'),(20,2,'CourseManage.aspx','default'),(21,2,'CourseManage.aspx','list'),(22,2,'Default.aspx','*'),(23,2,'Home.aspx','*'),(24,2,'PaidGroupManage.aspx','default'),(25,2,'PaidGroupManage.aspx','list'),(26,2,'PaymentManage.aspx','*'),(27,2,'PromotionManage.aspx','default'),(28,2,'PromotionManage.aspx','list'),(29,2,'RegisterCourse.aspx','*'),(30,2,'RegistrationManage.aspx','default'),(31,2,'RegistrationManage.aspx','list'),(32,2,'RegistrationManage.aspx','init_print_card'),(33,2,'RegistrationManage.aspx','init_print_receipt'),(34,2,'RegistrationManage.aspx','init_print_all'),(35,2,'ReportDailyRegistration.aspx','*'),(36,2,'ReportRegis.aspx','*'),(37,2,'RoomManage.aspx','default'),(38,2,'RoomManage.aspx','list'),(39,2,'StudentManage.aspx','*'),(40,2,'TeacherManage.aspx','default'),(41,2,'TeacherManage.aspx','list'),(42,3,'Administrator.aspx','default'),(43,3,'Administrator.aspx','list'),(44,3,'BranchManage.aspx','default'),(45,3,'BranchManage.aspx','list'),(46,3,'CourseManage.aspx','default'),(47,3,'CourseManage.aspx','list'),(48,3,'Default.aspx','*'),(49,3,'Home.aspx','*'),(50,3,'PromotionManage.aspx','default'),(51,3,'PromotionManage.aspx','list'),(52,3,'RegisterCourse.aspx','*'),(53,3,'RegistrationManage.aspx','default'),(54,3,'RegistrationManage.aspx','list'),(55,3,'RegistrationManage.aspx','init_print_card'),(56,3,'RegistrationManage.aspx','init_print_receipt'),(57,3,'RegistrationManage.aspx','init_print_all'),(58,3,'RoomManage.aspx','default'),(59,3,'RoomManage.aspx','list'),(60,3,'StudentManage.aspx','*'),(61,3,'TeacherManage.aspx','default'),(62,3,'TeacherManage.aspx','list'),(63,2,'CourseManage.aspx','view'),(64,2,'CourseManage.aspx','init_print'),(65,2,'CourseManage.aspx','save_as_excel'),(66,3,'CourseManage.aspx','view'),(67,3,'CourseManage.aspx','init_print'),(68,3,'CourseManage.aspx','save_as_excel'),(69,3,'ReportDailyRegistration.aspx','*');
/*!40000 ALTER TABLE `authorization` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `branch`
--

DROP TABLE IF EXISTS `branch`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `branch` (
  `branch_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `branch_name` varchar(100) NOT NULL,
  `address` varchar(200) NOT NULL,
  `tel` varchar(45) NOT NULL,
  `supervisor` varchar(100) NOT NULL,
  `img` varchar(50) NOT NULL DEFAULT 'noimg.jpg',
  `branch_code` varchar(10) NOT NULL DEFAULT 'noimg.jpg',
  PRIMARY KEY (`branch_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=tis620 PACK_KEYS=1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `branch`
--

LOCK TABLES `branch` WRITE;
/*!40000 ALTER TABLE `branch` DISABLE KEYS */;
INSERT INTO `branch` VALUES (1,'theBTS @สีลม','64 อาคารสีลม 64 ชั้น 5 ถนนสีลม แขวงสุริยวงศ์ เขตบางรัก กรุงเทพมหานคร 10500','02 - 632 - 9884-5 Fax:. 02 - 632 - 9965','จิตติวัฒน์ ทองนวล','62232900.jpg','SL');
/*!40000 ALTER TABLE `branch` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `btsconfig`
--

DROP TABLE IF EXISTS `btsconfig`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `btsconfig` (
  `name` varchar(50) NOT NULL,
  `value` varchar(1000) CHARACTER SET latin1 DEFAULT NULL,
  PRIMARY KEY (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `btsconfig`
--

LOCK TABLES `btsconfig` WRITE;
/*!40000 ALTER TABLE `btsconfig` DISABLE KEYS */;
INSERT INTO `btsconfig` VALUES ('LOG_SEVERITY','INFO');
/*!40000 ALTER TABLE `btsconfig` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `course`
--

DROP TABLE IF EXISTS `course`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `course` (
  `course_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `bts_course_id` varchar(50) NOT NULL,
  `course_name` varchar(100) NOT NULL,
  `short_name` varchar(50) NOT NULL,
  `course_type` varchar(50) NOT NULL DEFAULT 'คอร์สสด',
  `course_desc` varchar(1000) NOT NULL,
  `room_id` int(10) unsigned NOT NULL,
  `teacher_id` int(10) unsigned NOT NULL,
  `paid_group_id` int(10) unsigned NOT NULL,
  `category` varchar(50) NOT NULL,
  `start_date` date DEFAULT '2010-08-20',
  `end_date` date DEFAULT '2010-08-29',
  `open_time` varchar(50) NOT NULL,
  `day_of_week` varchar(50) NOT NULL COMMENT 'Mon,Tue,Wed,Thu,Fri,Sat,Sun',
  `cost` int(10) unsigned NOT NULL DEFAULT '0',
  `seat_limit` int(11) NOT NULL DEFAULT '40',
  `bank_regis_limit` int(11) NOT NULL DEFAULT '30',
  `image` varchar(50) DEFAULT NULL,
  `is_active` int(4) NOT NULL DEFAULT '1',
  `is_paid` int(11) NOT NULL DEFAULT '0' COMMENT '0=notpaid 1=paid',
  PRIMARY KEY (`course_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3252 DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `course`
--

LOCK TABLES `course` WRITE;
/*!40000 ALTER TABLE `course` DISABLE KEYS */;
/*!40000 ALTER TABLE `course` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paid_group`
--

DROP TABLE IF EXISTS `paid_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `paid_group` (
  `paid_group_id` int(10) unsigned NOT NULL,
  `name` varchar(100) NOT NULL DEFAULT '' COMMENT 'ex: ;100000:10;500000;20;1000000:30;',
  `rate_info` varchar(300) NOT NULL DEFAULT '' COMMENT 'ex: ;100000:10;500000;20;1000000:30;',
  `current_round` int(10) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`paid_group_id`),
  KEY `promotiom_id` (`paid_group_id`),
  KEY `course_id` (`rate_info`)
) ENGINE=InnoDB DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paid_group`
--

LOCK TABLES `paid_group` WRITE;
/*!40000 ALTER TABLE `paid_group` DISABLE KEYS */;
INSERT INTO `paid_group` (`paid_group_id`, `name`, `rate_info`, `current_round`) VALUES (1, 'กลุ่มอาจารย์1', '0:100', 0);
/*!40000 ALTER TABLE `paid_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paid_group_teacher_mapping`
--

DROP TABLE IF EXISTS `paid_group_teacher_mapping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `paid_group_teacher_mapping` (
  `paid_group_id` int(10) unsigned NOT NULL,
  `teacher_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`paid_group_id`,`teacher_id`),
  KEY `paid_group_id` (`paid_group_id`),
  KEY `teacher_id` (`teacher_id`)
) ENGINE=InnoDB DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paid_group_teacher_mapping`
--

LOCK TABLES `paid_group_teacher_mapping` WRITE;
/*!40000 ALTER TABLE `paid_group_teacher_mapping` DISABLE KEYS */;
INSERT INTO `paid_group_teacher_mapping` (`paid_group_id`, `teacher_id`) VALUES (1, 1);
/*!40000 ALTER TABLE `paid_group_teacher_mapping` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `payment`
--

DROP TABLE IF EXISTS `payment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payment`
--

LOCK TABLES `payment` WRITE;
/*!40000 ALTER TABLE `payment` DISABLE KEYS */;
/*!40000 ALTER TABLE `payment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `payment_history`
--

DROP TABLE IF EXISTS `payment_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payment_history`
--

LOCK TABLES `payment_history` WRITE;
/*!40000 ALTER TABLE `payment_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `payment_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `promotion`
--

DROP TABLE IF EXISTS `promotion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `promotion` (
  `promotion_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `promotion_name` varchar(100) NOT NULL,
  `promotion_desc` varchar(1000) NOT NULL,
  `cost` int(10) unsigned NOT NULL DEFAULT '0',
  `is_active` int(4) NOT NULL DEFAULT '1',
  `is_paid` int(11) NOT NULL DEFAULT '0' COMMENT '0=notpaid 1=paid',
  PRIMARY KEY (`promotion_id`)
) ENGINE=InnoDB DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `promotion`
--

LOCK TABLES `promotion` WRITE;
/*!40000 ALTER TABLE `promotion` DISABLE KEYS */;
/*!40000 ALTER TABLE `promotion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `promotion_course_mapping`
--

DROP TABLE IF EXISTS `promotion_course_mapping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `promotion_course_mapping` (
  `promotion_id` int(10) unsigned NOT NULL,
  `course_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`promotion_id`,`course_id`),
  KEY `promotiom_id` (`promotion_id`),
  KEY `course_id` (`course_id`)
) ENGINE=InnoDB DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `promotion_course_mapping`
--

LOCK TABLES `promotion_course_mapping` WRITE;
/*!40000 ALTER TABLE `promotion_course_mapping` DISABLE KEYS */;
/*!40000 ALTER TABLE `promotion_course_mapping` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `registration`
--

DROP TABLE IF EXISTS `registration`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `registration` (
  `regis_id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'uniqe per course/pro - student',
  `transaction_id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'one transaction consists of multiple register_id',
  `transaction_code` varchar(20) NOT NULL DEFAULT '',
  `regis_date` datetime NOT NULL,
  `student_id` int(10) unsigned NOT NULL,
  `course_id` int(10) unsigned NOT NULL DEFAULT '0',
  `discounted_cost` int(10) unsigned NOT NULL COMMENT 'cost after discount. will be devided by ratio if registered by promotion',
  `promotion_id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT '=0 if register by course. no promotion',
  `full_cost` int(10) unsigned NOT NULL COMMENT 'equal cost if no promotion',
  `seat_no` varchar(20) DEFAULT '',
  `note` varchar(200) NOT NULL DEFAULT '',
  `branch_id` int(10) unsigned NOT NULL COMMENT 'transaction at which branch',
  `paid_method` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Cash=0, Transfer=1, Credit=2, SCB=3',
  `paid_round` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'round to calculate accumulate income. will be increase once reset income.',
  `paid_date` datetime NOT NULL,
  `username` varchar(50) NOT NULL DEFAULT '',
  `is_paid` int(10) unsigned NOT NULL DEFAULT '0',
  `status` int(10) unsigned NOT NULL DEFAULT '0' COMMENT '0=valid 1=cancelled',
  PRIMARY KEY (`regis_id`),
  KEY `promotiom_id` (`student_id`),
  KEY `course_id` (`course_id`)
) ENGINE=InnoDB AUTO_INCREMENT=42119 DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `registration`
--

LOCK TABLES `registration` WRITE;
/*!40000 ALTER TABLE `registration` DISABLE KEYS */;
/*!40000 ALTER TABLE `registration` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `role` (
  `role_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`role_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (1,'Admin'),(2,'Management'),(3,'Front Staff');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `room`
--

DROP TABLE IF EXISTS `room`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `room`
--

LOCK TABLES `room` WRITE;
/*!40000 ALTER TABLE `room` DISABLE KEYS */;
INSERT INTO `room` (`room_id`, `name`, `branch_id`, `seat_no`, `img`, `description`) VALUES (30, 'ห้องเรียน1', 1, 50, 'noimg.jpg', '');
/*!40000 ALTER TABLE `room` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `student`
--

DROP TABLE IF EXISTS `student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `student` (
  `student_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `firstname` varchar(50) NOT NULL,
  `surname` varchar(50) NOT NULL,
  `nickname` varchar(20) NOT NULL,
  `tel` varchar(30) NOT NULL,
  `tel2` varchar(30) NOT NULL,
  `citizen_id` varchar(20) NOT NULL DEFAULT '',
  `email` varchar(50) NOT NULL,
  `school` varchar(50) DEFAULT NULL,
  `quiz` varchar(250) NOT NULL DEFAULT '' COMMENT '1;2;3;6:xxxxxxxx',
  `level` int(4) NOT NULL DEFAULT '1',
  `sex` varchar(6) NOT NULL,
  `birthday` date DEFAULT NULL,
  `addr` varchar(100) DEFAULT '''-''',
  `image` varchar(50) DEFAULT NULL,
  `create_date` date DEFAULT NULL,
  `is_active` int(4) NOT NULL DEFAULT '1',
  PRIMARY KEY (`student_id`)
) ENGINE=InnoDB AUTO_INCREMENT=15317 DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `student`
--

LOCK TABLES `student` WRITE;
/*!40000 ALTER TABLE `student` DISABLE KEYS */;
INSERT INTO `student` VALUES (15316,'1213','323','','','','','','','',1,'Male','2015-10-08','','noimg.jpg','2015-10-08',0);
/*!40000 ALTER TABLE `student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `teacher`
--

DROP TABLE IF EXISTS `teacher`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `teacher` (
  `teacher_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `firstname` varchar(50) NOT NULL,
  `surname` varchar(50) NOT NULL,
  `tel` varchar(30) DEFAULT '',
  `citizen_id` varchar(20) NOT NULL DEFAULT '',
  `email` varchar(100) DEFAULT '',
  `sex` varchar(6) NOT NULL,
  `birthday` date DEFAULT NULL,
  `addr` varchar(100) DEFAULT '''-''',
  `image` varchar(50) DEFAULT NULL,
  `subject` varchar(100) DEFAULT NULL,
  `is_active` int(4) NOT NULL DEFAULT '1',
  PRIMARY KEY (`teacher_id`)
) ENGINE=InnoDB AUTO_INCREMENT=172 DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `teacher`
--

LOCK TABLES `teacher` WRITE;
/*!40000 ALTER TABLE `teacher` DISABLE KEYS */;
INSERT INTO `teacher` VALUES (1,'อ.บัณฑิตย์','ฝอยทอง','0816342179','','','Male','2010-07-12','','noimg.jpg','',1);
/*!40000 ALTER TABLE `teacher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user` (
  `username` varchar(50) CHARACTER SET latin1 NOT NULL,
  `user_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `passwd` varchar(255) CHARACTER SET latin1 NOT NULL,
  `role_id` int(10) unsigned DEFAULT NULL,
  `firstname` varchar(50) DEFAULT NULL,
  `surname` varchar(50) DEFAULT NULL,
  `is_valid` int(4) unsigned NOT NULL DEFAULT '1',
  `branch_id` int(10) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`username`),
  UNIQUE KEY `user_id_UNIQUE` (`user_id`),
  KEY `FK_user_role_id` (`role_id`),
  CONSTRAINT `FK_user_role_id` FOREIGN KEY (`role_id`) REFERENCES `role` (`role_id`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES ('joke', 35, '2f959fc21a0b5e77303f2ef9ed075cf0', 1, 'joke', 'joke', 1, 1),('kizze',11,'cc03e747a6afbbcbf8be7668acfebee5',3,'Kittipong','test123',1,1),('netta',15,'cc03e747a6afbbcbf8be7668acfebee5',1,'Weerawat','Seetalalai',1,1),('pang', 34, 'd99c0f4dccf8de7bee05ed3ad4c4f26a', 1, 'ศุภาวรรณ', 'ช่วยกันจักร์', 1, 1),('root',25,'4abfba1ab7fa0285a1784f42d6a38b56',1,'root','root',1,1);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'bts'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-10-08 22:13:50

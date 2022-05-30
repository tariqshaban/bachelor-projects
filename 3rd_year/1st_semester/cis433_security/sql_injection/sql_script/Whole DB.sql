-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 23, 2019 at 02:42 PM
-- Server version: 10.1.38-MariaDB
-- PHP Version: 7.3.3

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `student`
--
CREATE DATABASE IF NOT EXISTS `student` DEFAULT CHARACTER SET utf8 COLLATE utf8_bin;
USE `student`;

-- --------------------------------------------------------

--
-- Table structure for table `password`
--

CREATE TABLE `password` (
  `std_id` int(11) NOT NULL,
  `password` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `password`
--

INSERT INTO `password` (`std_id`, `password`) VALUES
(12333, 'e99a18c428cb38d5f260853678922e03'),
(12345, 'e10adc3949ba59abbe56e057f20f883e'),
(12348, 'f9ada32006d6ee6ae9e9e2067a7ab5ba'),
(12362, 'a9992c7a1ebbd70cb48968fb954732d3'),
(18345, '2f9d69b3306796ed3f4ed578ad5cf8c5'),
(99999, '21232f297a57a5a743894a0e4a801fc3');

-- --------------------------------------------------------

--
-- Table structure for table `student`
--

CREATE TABLE `student` (
  `id` int(6) NOT NULL,
  `first_name` varchar(200) NOT NULL,
  `last_name` varchar(200) NOT NULL,
  `grade` float DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `student`
--

INSERT INTO `student` (`id`, `first_name`, `last_name`, `grade`) VALUES
(12333, 'samy', 'khaled', 80),
(12345, 'saleh', 'ahmad', 50),
(12348, 'ramy', 'ahmad', 25),
(12362, 'ali', 'tariq', 50),
(18345, 'saeed', 'khaled', 20),
(99999, 'teacher', 'teacher', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `uac`
--

CREATE TABLE `uac` (
  `ID` int(11) NOT NULL,
  `Read_AC` tinyint(1) NOT NULL,
  `Write_AC` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `uac`
--

INSERT INTO `uac` (`ID`, `Read_AC`, `Write_AC`) VALUES
(12333, 1, 0),
(12345, 1, 0),
(12348, 1, 0),
(12362, 1, 0),
(18345, 1, 0),
(99999, 1, 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `password`
--
ALTER TABLE `password`
  ADD PRIMARY KEY (`std_id`);

--
-- Indexes for table `student`
--
ALTER TABLE `student`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `uac`
--
ALTER TABLE `uac`
  ADD PRIMARY KEY (`ID`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `password`
--
ALTER TABLE `password`
  ADD CONSTRAINT `Password_ibfk_1` FOREIGN KEY (`std_id`) REFERENCES `student` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `uac`
--
ALTER TABLE `uac`
  ADD CONSTRAINT `FK_PersonOrder` FOREIGN KEY (`ID`) REFERENCES `password` (`std_id`);
--
-- Database: `student_bak`
--
CREATE DATABASE IF NOT EXISTS `student_bak` DEFAULT CHARACTER SET utf8 COLLATE utf8_bin;
USE `student_bak`;

-- --------------------------------------------------------

--
-- Table structure for table `password`
--

CREATE TABLE `password` (
  `std_id` int(11) NOT NULL,
  `password` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `password`
--

INSERT INTO `password` (`std_id`, `password`) VALUES
(12333, 'e99a18c428cb38d5f260853678922e03'),
(12345, 'e10adc3949ba59abbe56e057f20f883e'),
(12348, 'f9ada32006d6ee6ae9e9e2067a7ab5ba'),
(12362, 'a9992c7a1ebbd70cb48968fb954732d3'),
(18345, '2f9d69b3306796ed3f4ed578ad5cf8c5'),
(99999, '21232f297a57a5a743894a0e4a801fc3');

-- --------------------------------------------------------

--
-- Table structure for table `student`
--

CREATE TABLE `student` (
  `id` int(6) NOT NULL,
  `first_name` varchar(200) NOT NULL,
  `last_name` varchar(200) NOT NULL,
  `grade` float DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `student`
--

INSERT INTO `student` (`id`, `first_name`, `last_name`, `grade`) VALUES
(12333, 'samy', 'khaled', 80),
(12345, 'saleh', 'ahmad', 50),
(12348, 'ramy', 'ahmad', 25),
(12362, 'ali', 'tariq', 50),
(18345, 'saeed', 'khaled', 20),
(99999, 'teacher', 'teacher', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `uac`
--

CREATE TABLE `uac` (
  `ID` int(11) NOT NULL,
  `Read_AC` tinyint(1) NOT NULL,
  `Write_AC` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `uac`
--

INSERT INTO `uac` (`ID`, `Read_AC`, `Write_AC`) VALUES
(12333, 1, 0),
(12345, 1, 0),
(12348, 1, 0),
(12362, 1, 0),
(18345, 1, 0),
(99999, 1, 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `password`
--
ALTER TABLE `password`
  ADD PRIMARY KEY (`std_id`);

--
-- Indexes for table `student`
--
ALTER TABLE `student`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `uac`
--
ALTER TABLE `uac`
  ADD PRIMARY KEY (`ID`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `password`
--
ALTER TABLE `password`
  ADD CONSTRAINT `Password_ibfk_1` FOREIGN KEY (`std_id`) REFERENCES `student` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 13, 2018 at 08:42 PM
-- Server version: 10.1.37-MariaDB
-- PHP Version: 7.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `library`
--
CREATE DATABASE IF NOT EXISTS `library` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `library`;

-- --------------------------------------------------------

--
-- Table structure for table `book`
--

CREATE TABLE `book` (
  `ID` int(30) NOT NULL,
  `Title` varchar(30) NOT NULL,
  `Publisher` varchar(30) NOT NULL,
  `Edition` varchar(30) NOT NULL,
  `Price` int(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `book`
--

INSERT INTO `book` (`ID`, `Title`, `Publisher`, `Edition`, `Price`) VALUES
(111, 'Computer Skills', 'Random House', 'First', 21),
(112, 'Introduction to C++', 'Willy and Sons', 'First', 15),
(114, 'Database Concepts', 'Random House', 'Second', 33),
(211, 'Guide to Oracle', 'Random House', 'Third', 27),
(421, 'Data Structure', 'Pearson', 'Second', 42);

-- --------------------------------------------------------

--
-- Table structure for table `borrower`
--

CREATE TABLE `borrower` (
  `Num` int(30) NOT NULL,
  `Name` varchar(30) NOT NULL,
  `Phone` int(13) NOT NULL,
  `BDate` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `borrower`
--

INSERT INTO `borrower` (`Num`, `Name`, `Phone`, `BDate`) VALUES
(1, 'Rami', 2545644, '28-May-88'),
(2, 'Ola', 9872422, '24-Feb-87'),
(5, 'Eyad', 5489720, '10-Aug-85'),
(7, 'Ahmad', 3448445, '16-Oct-91'),
(9, 'Qusai', 5436247, '03-Nov-96');

-- --------------------------------------------------------

--
-- Table structure for table `loan`
--

CREATE TABLE `loan` (
  `ID` int(30) NOT NULL,
  `Num` int(30) NOT NULL,
  `Out_Date` varchar(30) NOT NULL,
  `Due_Date` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `loan`
--

INSERT INTO `loan` (`ID`, `Num`, `Out_Date`, `Due_Date`) VALUES
(111, 5, '25-Oct-13', '04-Nov-13'),
(111, 7, '27-Oct-13', '05-Nov-13'),
(211, 5, '25-Oct-13', '10-Nov-13'),
(421, 1, '30-Oct-13', '13-Nov-13');

-- --------------------------------------------------------

--
-- Table structure for table `sell`
--

CREATE TABLE `sell` (
  `ID` int(30) NOT NULL,
  `Num` int(30) NOT NULL,
  `Sdate` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `sell`
--

INSERT INTO `sell` (`ID`, `Num`, `Sdate`) VALUES
(111, 5, '02-Nov-13'),
(114, 9, '04-Nov-13');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `User` varchar(20) NOT NULL,
  `Pass` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`User`, `Pass`) VALUES
('admin', 'admin');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `book`
--
ALTER TABLE `book`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `borrower`
--
ALTER TABLE `borrower`
  ADD PRIMARY KEY (`Num`);

--
-- Indexes for table `loan`
--
ALTER TABLE `loan`
  ADD KEY `ID` (`ID`),
  ADD KEY `Num` (`Num`);

--
-- Indexes for table `sell`
--
ALTER TABLE `sell`
  ADD KEY `ID` (`ID`),
  ADD KEY `Num` (`Num`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`User`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `loan`
--
ALTER TABLE `loan`
  ADD CONSTRAINT `loan_ibfk_1` FOREIGN KEY (`ID`) REFERENCES `book` (`ID`),
  ADD CONSTRAINT `loan_ibfk_2` FOREIGN KEY (`Num`) REFERENCES `borrower` (`Num`);

--
-- Constraints for table `sell`
--
ALTER TABLE `sell`
  ADD CONSTRAINT `sell_ibfk_1` FOREIGN KEY (`ID`) REFERENCES `book` (`ID`),
  ADD CONSTRAINT `sell_ibfk_2` FOREIGN KEY (`Num`) REFERENCES `borrower` (`Num`);
--
-- Database: `messages`
--
CREATE DATABASE IF NOT EXISTS `messages` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `messages`;

-- --------------------------------------------------------

--
-- Table structure for table `message`
--

CREATE TABLE `message` (
  `Sender` varchar(20) NOT NULL,
  `Receiver` varchar(20) NOT NULL,
  `Content` varchar(200) NOT NULL,
  `Is_read` tinyint(1) NOT NULL,
  `Time` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `User` varchar(20) NOT NULL,
  `Pass` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`User`, `Pass`) VALUES
('user1', 'user1'),
('admin', 'admin');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`User`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

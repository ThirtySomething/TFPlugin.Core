CREATE TABLE IF NOT EXISTS `MType` (
	`ID` BIGINT(20) NOT NULL AUTO_INCREMENT,
	`Name` VARCHAR(255) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`ID`)
) COLLATE='utf8_general_ci' ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS `MUnit` (
	`ID` BIGINT(20) NOT NULL AUTO_INCREMENT,
	`Name` VARCHAR(255) NULL DEFAULT NULL COLLATE 'utf8mb4_general_ci',
	PRIMARY KEY (`ID`)
) COLLATE='utf8_general_ci' ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS `MValue` (
	`ID` BIGINT(20) NOT NULL AUTO_INCREMENT,
	`TypeID` BIGINT(20) NOT NULL,
	`Value` DOUBLE NOT NULL,
	`UnitID` BIGINT(20) NOT NULL,
	`RecordTime` TEXT NOT NULL DEFAULT current_timestamp(),
	PRIMARY KEY (`ID`),
	INDEX `FK_MValue_MType_TypeID` (`TypeID`),
	INDEX `FK_MValue_MUnit_UnitID` (`UnitID`),
	CONSTRAINT `FK_MValue_MType_TypeID` FOREIGN KEY (`TypeID`) REFERENCES `mtype` (`ID`),
	CONSTRAINT `FK_MValue_MUnit_UnitID` FOREIGN KEY (`UnitID`) REFERENCES `munit` (`ID`)
) COLLATE='utf8_general_ci' ENGINE=InnoDB;

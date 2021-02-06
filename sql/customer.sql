CREATE TABLE Customer
(
	CUSTOMER_ID INT IDENTITY(1,1) PRIMARY KEY /* {trait:means.identity; trait:is.dataFormat.integer} */,
	CUSTOMER_NAME VARCHAR(50) NOT NULL /* {trait:means.fullname; trait:means.content.text} */
);

CREATE TABLE CustomerAddresses
(
	CUSTOMER_ADDRESS_ID INT IDENTITY(1,1) PRIMARY KEY,
	CUSTOMER_ID INT NOT NULL /* {trait:means.identity; trait:is.dataFormat.integer} */,
	LINE VARCHAR(100) NOT NULL /* {trait:means.content.text(value, pattern=st.*)} */,
	STATE CHAR(2),
	COUNTRY CHAR(3),
	FOREIGN KEY(CUSTOMER_ID) REFERENCES Customer(CUSTOMER_ID)
);

CREATE TABLE SpecialCustomer /* {extends:Customer; description: VIP Customer} */(
	SPECIAL_CARD_NUMBER CHAR(15) NOT NULL /* {displayName: VIP Card} */
);
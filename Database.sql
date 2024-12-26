CREATE TABLE UserType (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL UNIQUE
);

CREATE TABLE User (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    email TEXT NOT NULL UNIQUE,
    passwordHash TEXT NOT NULL,
    createdAt TEXT NOT NULL,
    lastLogin TEXT,
    userTypeId INTEGER NOT NULL,
    FOREIGN KEY (id) REFERENCES UserType(id) ON DELETE CASCADE
);

CREATE TABLE File (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    fileName TEXT,
    encryptedUrl TEXT,
    publicKey TEXT,
    signature TEXT,
    uploaderId INTEGER NOT NULL,
    uploadedAt TEXT NOT NULL,
    FOREIGN KEY (uploaderId) REFERENCES User(id) ON DELETE CASCADE
);

CREATE TABLE FileAccess (
    fileId INTEGER NOT NULL,
    userId INTEGER,
    userEmail TEXT NOT NULL,
    accessLevel TEXT,
    grantedAt TEXT NOT NULL,
    PRIMARY KEY (fileId, userEmail),
    FOREIGN KEY (fileId) REFERENCES File(id) ON DELETE CASCADE,
    FOREIGN KEY (userId) REFERENCES User(id) ON DELETE CASCADE
);

CREATE TABLE ActivityLog (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    userId INTEGER,
    fileId INTEGER,
    action TEXT,
    timestamp TEXT,
    FOREIGN KEY (userId) REFERENCES User(id) ON DELETE CASCADE,
    FOREIGN KEY (fileId) REFERENCES File(id) ON DELETE CASCADE
);
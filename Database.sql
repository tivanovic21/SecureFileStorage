CREATE TABLE User (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    email TEXT NOT NULL UNIQUE,
    passwordHash TEXT NOT NULL,
    createdAt TEXT NOT NULL,
    lastLogin TEXT
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
    userId INTEGER NOT NULL,
    accessLevel TEXT,
    grantedAt TEXT NOT NULL,
    PRIMARY KEY (fileId, userId),
    FOREIGN KEY (fileId) REFERENCES File(id) ON DELETE CASCADE,
    FOREIGN KEY (userId) REFERENCES User(id) ON DELETE CASCADE
);

CREATE TABLE ActivityLogs (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    userId INTEGER,
    fileId INTEGER,
    action TEXT,
    timestamp TEXT,
    FOREIGN KEY (userId) REFERENCES User(id) ON DELETE CASCADE,
    FOREIGN KEY (fileId) REFERENCES File(id) ON DELETE CASCADE
);
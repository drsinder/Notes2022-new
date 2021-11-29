# Notes2022.Shared Project

This project contains database models (Entities folder) and client/server communication models (NotesModels folder).
Also contained in NotesModels is a Data Access Layer (DAL.cs) which provides encapsulates the calls to the Controllers in the Server.

    /// This is a Data Access Layer for Notes2022.  All UI interaction with 
    /// the Data on the Server takes place through here.  Data is pulled from 
    /// and pushed to the server through these methods.
    /// 
    /// The first parameter of every method is an HttpClient which enables 
    /// the DAL to talk to the Server.  On the Client side, HttpClient
    /// is "Injected" into the pages that require it.  (Most pages do.)
    /// 
    /// These methods are functionally oriented and not intended to directly
    /// access entities (Tables/Rows) although they do effect them and
    /// return them as a part of functional requests.
    /// 
    /// These methods will silently fail to do the requested action in some
    /// cases.  For example:  If the user does not have read access to a
    /// file they may not get data back.  If they do not have write access
    /// an attempt to create a note will fail silently...  The User Interface
    /// should prevent such attempts, but the server may enforce...
    /// 
    /// This is a rather minimal set.  Other functional features could call
    /// for additional methods and matching controllers on the server.
    /// For example: The database has a table that can be used to "Mark"
    /// notes for later output.  But the methods are not yet here or on
    /// the server.  Still other functions might call for added columns or
    /// even tables.  This can be done although it's more involved.
    /// 
    /// For a summary view of the datatables involved see the file
    /// NoteFile.cd under the Entities folder in this project.  For a
    /// more detailed look see the classes representing those entities
    /// in the same folder. These classes directly represent the database.
    /// The database was created FROM these classes in a "code-first"
    /// approach.  See the Notes2022.Server project folder Data, class
    /// NotesDbContext.cs.  This defines the database tables and how they
    /// relate to one another.  (Note:  The Database has a few other tables
    /// added for user Identity, Roles, etc...  These begin with "AspNet".
    /// Another extra set of tables begin with "HangFire."  These are added
    /// by the background job processor, Hangfire.)  Final note: The
    /// "AspNetUsers" table has had extra columns added for user
    /// preferences.  See the Models folder in the Notes2022.Server
    /// project for the added columns.
    /// 
    /// Sometimes the communication with the server is through non-database
    /// classes.  These are found in the NotesMoels folder in this project.
    /// See the classes or the InternalModels.cd file for a summary. 


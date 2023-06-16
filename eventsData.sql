-- Insert events for EventType "createNewUser"
INSERT INTO Events (EventType, Time, ProjectId, UserName)
VALUES ('createNewUser', '2023-06-01 10:00:00', NULL, 'User1');

INSERT INTO Events (EventType, Time, ProjectId, UserName)
VALUES ('createNewUser', '2023-06-02 09:30:00', NULL, 'User2');

INSERT INTO Events (EventType, Time, ProjectId, UserName)
VALUES ('createNewUser', '2023-06-03 11:45:00', NULL, 'User3');

INSERT INTO Events (EventType, Time, ProjectId, UserName)
VALUES ('createNewUser', '2023-06-03 13:15:00', NULL, 'User4');

INSERT INTO Events (EventType, Time, ProjectId, UserName)
VALUES ('createNewUser', '2023-06-04 14:30:00', NULL, 'User5');

-- Insert events for EventType "createNewProject"
INSERT INTO Events (EventType, Time, ProjectId, UserName)
VALUES ('createNewProject', '2023-06-05 10:30:00', 1, NULL);

INSERT INTO Events (EventType, Time, ProjectId, UserName)
VALUES ('createNewProject', '2023-06-05 11:00:00', 2, NULL);

INSERT INTO Events (EventType, Time, ProjectId, UserName)
VALUES ('createNewProject', '2023-06-06 15:45:00', 3, NULL);

INSERT INTO Events (EventType, Time, ProjectId, UserName)
VALUES ('createNewProject', '2023-06-06 16:30:00', 4, NULL);

INSERT INTO Events (EventType, Time, ProjectId, UserName)
VALUES ('createNewProject', '2023-06-06 17:15:00', 5, NULL);

INSERT INTO Events (EventType, Time, ProjectId, UserName)
VALUES ('createNewProject', '2023-06-07 09:00:00', 6, NULL);

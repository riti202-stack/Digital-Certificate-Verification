CREATE OR REPLACE PACKAGE cert_pkg AS

    TYPE ref_cursor IS REF CURSOR;

    FUNCTION  login_admin(p_username VARCHAR2, p_password VARCHAR2) RETURN NUMBER;
    FUNCTION  login_student(p_email VARCHAR2, p_password VARCHAR2) RETURN NUMBER;

    PROCEDURE get_departments(p_cursor OUT ref_cursor);
    PROCEDURE get_certificate_types(p_cursor OUT ref_cursor);
    PROCEDURE get_students_lookup(p_cursor OUT ref_cursor);

    PROCEDURE add_student(p_name VARCHAR2, p_email VARCHAR2, p_password VARCHAR2,
                          p_phone VARCHAR2, p_dept_id NUMBER);
    PROCEDURE update_student(p_student_id NUMBER, p_name VARCHAR2, p_email VARCHAR2,
                             p_phone VARCHAR2, p_dept_id NUMBER);
    PROCEDURE delete_student(p_student_id NUMBER);
    PROCEDURE get_all_students(p_cursor OUT ref_cursor);
    PROCEDURE get_student_by_id(p_student_id NUMBER, p_cursor OUT ref_cursor);

    PROCEDURE add_certificate(p_code VARCHAR2, p_student_id NUMBER, p_type_id NUMBER,
                              p_title VARCHAR2, p_issue_date DATE, p_expiry_date DATE,
                              p_authority VARCHAR2, p_status VARCHAR2);
    PROCEDURE update_certificate(p_cert_id NUMBER, p_title VARCHAR2, p_issue_date DATE,
                                 p_expiry_date DATE, p_authority VARCHAR2, p_status VARCHAR2);
    PROCEDURE delete_certificate(p_cert_id NUMBER);
    PROCEDURE get_all_certificates(p_cursor OUT ref_cursor);
    PROCEDURE get_certificate_by_id(p_cert_id NUMBER, p_cursor OUT ref_cursor);

    PROCEDURE search_certificates(p_keyword VARCHAR2, p_cursor OUT ref_cursor);
    PROCEDURE filter_certificates(p_dept_ids VARCHAR2, p_type_ids VARCHAR2,
                                  p_statuses VARCHAR2, p_institution VARCHAR2,
                                  p_year VARCHAR2, p_from_date DATE, p_to_date DATE,
                                  p_cursor OUT ref_cursor);

    PROCEDURE verify_certificate(p_code VARCHAR2, p_verifier_name VARCHAR2,
                                 p_verifier_email VARCHAR2, p_cursor OUT ref_cursor);

    PROCEDURE submit_request(p_student_id NUMBER, p_type_id NUMBER, p_title VARCHAR2,
                             p_authority VARCHAR2, p_pref_date DATE, p_remarks VARCHAR2);
    PROCEDURE get_student_requests(p_student_id NUMBER, p_cursor OUT ref_cursor);
    PROCEDURE get_pending_requests(p_cursor OUT ref_cursor);
    PROCEDURE approve_request(p_request_id NUMBER, p_admin_id NUMBER, p_code VARCHAR2);
    PROCEDURE reject_request(p_request_id NUMBER, p_admin_id NUMBER);

    PROCEDURE get_dashboard_stats(p_total_students OUT NUMBER, p_total_certs OUT NUMBER,
                                  p_expired_certs OUT NUMBER, p_pending_requests OUT NUMBER);

END cert_pkg;

/


CREATE OR REPLACE PACKAGE BODY cert_pkg AS

    FUNCTION login_admin(p_username VARCHAR2, p_password VARCHAR2) RETURN NUMBER IS
        v_id NUMBER;
    BEGIN
        SELECT admin_id INTO v_id FROM Admins
        WHERE username = p_username AND password = p_password;
        RETURN v_id;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN RETURN -1;
    END;

    FUNCTION login_student(p_email VARCHAR2, p_password VARCHAR2) RETURN NUMBER IS
        v_id NUMBER;
    BEGIN
        SELECT student_id INTO v_id FROM Students
        WHERE email = p_email AND password = p_password;
        RETURN v_id;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN RETURN -1;
    END;

    PROCEDURE get_departments(p_cursor OUT ref_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT department_id, department_name FROM Departments ORDER BY department_name;
    END;

    PROCEDURE get_certificate_types(p_cursor OUT ref_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT type_id, type_name FROM Certificate_Types ORDER BY type_name;
    END;

    PROCEDURE get_students_lookup(p_cursor OUT ref_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT student_id, student_name FROM Students ORDER BY student_name;
    END;

    PROCEDURE add_student(p_name VARCHAR2, p_email VARCHAR2, p_password VARCHAR2,
                          p_phone VARCHAR2, p_dept_id NUMBER) IS
    BEGIN
        INSERT INTO Students(student_id, student_name, email, password, phone, department_id)
        VALUES (student_seq.NEXTVAL, p_name, p_email, p_password, p_phone, p_dept_id);
    END;

    PROCEDURE update_student(p_student_id NUMBER, p_name VARCHAR2, p_email VARCHAR2,
                             p_phone VARCHAR2, p_dept_id NUMBER) IS
    BEGIN
        UPDATE Students
        SET student_name = p_name, email = p_email, phone = p_phone, department_id = p_dept_id
        WHERE student_id = p_student_id;
    END;

    PROCEDURE delete_student(p_student_id NUMBER) IS
    BEGIN
        DELETE FROM Students WHERE student_id = p_student_id;
    END;

    PROCEDURE get_all_students(p_cursor OUT ref_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT s.student_id, s.student_name, s.email, s.phone,
                   d.department_name, s.department_id
            FROM Students s
            LEFT JOIN Departments d ON s.department_id = d.department_id
            ORDER BY s.student_id;
    END;

    PROCEDURE get_student_by_id(p_student_id NUMBER, p_cursor OUT ref_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT student_id, student_name, email, phone, department_id
            FROM Students WHERE student_id = p_student_id;
    END;

    PROCEDURE add_certificate(p_code VARCHAR2, p_student_id NUMBER, p_type_id NUMBER,
                              p_title VARCHAR2, p_issue_date DATE, p_expiry_date DATE,
                              p_authority VARCHAR2, p_status VARCHAR2) IS
    BEGIN
        INSERT INTO Certificates(certificate_id, certificate_code, student_id, type_id,
                                 title, issue_date, expiry_date, issuing_authority, status)
        VALUES (cert_seq.NEXTVAL, p_code, p_student_id, p_type_id,
                p_title, p_issue_date, p_expiry_date, p_authority, p_status);
    END;

    PROCEDURE update_certificate(p_cert_id NUMBER, p_title VARCHAR2, p_issue_date DATE,
                                 p_expiry_date DATE, p_authority VARCHAR2, p_status VARCHAR2) IS
    BEGIN
        UPDATE Certificates
        SET title = p_title, issue_date = p_issue_date, expiry_date = p_expiry_date,
            issuing_authority = p_authority, status = p_status
        WHERE certificate_id = p_cert_id;
    END;

    PROCEDURE delete_certificate(p_cert_id NUMBER) IS
    BEGIN
        DELETE FROM Verifications WHERE certificate_id = p_cert_id;
        DELETE FROM Certificates WHERE certificate_id = p_cert_id;
    END;

    PROCEDURE get_all_certificates(p_cursor OUT ref_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT c.certificate_id, c.certificate_code, s.student_name,
                   ct.type_name, c.status
            FROM Certificates c
            JOIN Students s          ON c.student_id = s.student_id
            JOIN Certificate_Types ct ON c.type_id    = ct.type_id
            ORDER BY c.certificate_id DESC;
    END;

    PROCEDURE get_certificate_by_id(p_cert_id NUMBER, p_cursor OUT ref_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT certificate_code, student_id, type_id, title,
                   issue_date, expiry_date, issuing_authority, status
            FROM Certificates WHERE certificate_id = p_cert_id;
    END;

    PROCEDURE search_certificates(p_keyword VARCHAR2, p_cursor OUT ref_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT c.certificate_id, c.certificate_code, s.student_name,
                   ct.type_name, c.issuing_authority, c.issue_date,
                   c.status, d.department_name
            FROM Certificates c
            JOIN Students s           ON c.student_id  = s.student_id
            JOIN Certificate_Types ct ON c.type_id     = ct.type_id
            LEFT JOIN Departments d   ON s.department_id = d.department_id
            WHERE UPPER(c.certificate_code) LIKE '%' || UPPER(p_keyword) || '%'
               OR UPPER(s.student_name)     LIKE '%' || UPPER(p_keyword) || '%'
            ORDER BY c.certificate_id DESC;
    END;

    PROCEDURE filter_certificates(p_dept_ids VARCHAR2, p_type_ids VARCHAR2,
                                  p_statuses VARCHAR2, p_institution VARCHAR2,
                                  p_year VARCHAR2, p_from_date DATE, p_to_date DATE,
                                  p_cursor OUT ref_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT c.certificate_id, c.certificate_code, s.student_name,
                   ct.type_name, c.issuing_authority, c.issue_date,
                   c.status, d.department_name
            FROM Certificates c
            JOIN Students s           ON c.student_id    = s.student_id
            JOIN Certificate_Types ct ON c.type_id       = ct.type_id
            LEFT JOIN Departments d   ON s.department_id = d.department_id
            WHERE (p_dept_ids    IS NULL OR ',' || p_dept_ids || ','   LIKE '%,' || s.department_id || ',%')
              AND (p_type_ids    IS NULL OR ',' || p_type_ids || ','   LIKE '%,' || c.type_id || ',%')
              AND (p_statuses    IS NULL OR ',' || p_statuses || ','   LIKE '%,' || c.status || ',%')
              AND (p_institution IS NULL OR UPPER(c.issuing_authority) LIKE '%' || UPPER(p_institution) || '%')
              AND (p_year        IS NULL OR TO_CHAR(c.issue_date, 'YYYY') = p_year)
              AND (p_from_date   IS NULL OR c.issue_date >= p_from_date)
              AND (p_to_date     IS NULL OR c.issue_date <= p_to_date)
            ORDER BY c.certificate_id DESC;
    END;

    PROCEDURE verify_certificate(p_code VARCHAR2, p_verifier_name VARCHAR2,
                                 p_verifier_email VARCHAR2, p_cursor OUT ref_cursor) IS
        v_cert_id NUMBER;
    BEGIN
        BEGIN
            SELECT certificate_id INTO v_cert_id
            FROM Certificates WHERE certificate_code = p_code;

            INSERT INTO Verifications(verification_id, certificate_id, verifier_name,
                                      verifier_email, verification_date, remarks)
            VALUES (verify_seq.NEXTVAL, v_cert_id, p_verifier_name,
                    p_verifier_email, SYSDATE, 'Verified via portal');
        EXCEPTION
            WHEN NO_DATA_FOUND THEN NULL;
        END;

        OPEN p_cursor FOR
            SELECT c.certificate_code, s.student_name, ct.type_name,
                   d.department_name, c.issuing_authority, c.issue_date, c.expiry_date,
                   CASE WHEN c.expiry_date IS NOT NULL AND c.expiry_date < SYSDATE
                        THEN 'Expired' ELSE c.status END AS effective_status
            FROM Certificates c
            JOIN Students s           ON c.student_id    = s.student_id
            JOIN Certificate_Types ct ON c.type_id       = ct.type_id
            LEFT JOIN Departments d   ON s.department_id = d.department_id
            WHERE c.certificate_code = p_code;
    END;

    PROCEDURE submit_request(p_student_id NUMBER, p_type_id NUMBER, p_title VARCHAR2,
                             p_authority VARCHAR2, p_pref_date DATE, p_remarks VARCHAR2) IS
    BEGIN
        INSERT INTO Certificate_Requests(request_id, student_id, type_id, title,
                                         issuing_authority, preferred_issue_date,
                                         remarks, requested_date, status)
        VALUES (request_seq.NEXTVAL, p_student_id, p_type_id, p_title,
                p_authority, p_pref_date, p_remarks, SYSDATE, 'Pending');
    END;

    PROCEDURE get_student_requests(p_student_id NUMBER, p_cursor OUT ref_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT r.request_id, ct.type_name, r.title, r.requested_date, r.status
            FROM Certificate_Requests r
            JOIN Certificate_Types ct ON r.type_id = ct.type_id
            WHERE r.student_id = p_student_id
            ORDER BY r.request_id DESC;
    END;

    PROCEDURE get_pending_requests(p_cursor OUT ref_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT r.request_id, s.student_name, ct.type_name, r.title,
                   r.issuing_authority, r.preferred_issue_date, r.remarks, r.requested_date
            FROM Certificate_Requests r
            JOIN Students s           ON r.student_id = s.student_id
            JOIN Certificate_Types ct ON r.type_id    = ct.type_id
            WHERE r.status = 'Pending'
            ORDER BY r.requested_date;
    END;

    PROCEDURE approve_request(p_request_id NUMBER, p_admin_id NUMBER, p_code VARCHAR2) IS
        v_student_id NUMBER;
        v_type_id    NUMBER;
        v_title      VARCHAR2(200);
        v_authority  VARCHAR2(100);
        v_pref_date  DATE;
        v_new_cert_id NUMBER;
    BEGIN
        SELECT student_id, type_id, title, issuing_authority, preferred_issue_date
        INTO v_student_id, v_type_id, v_title, v_authority, v_pref_date
        FROM Certificate_Requests WHERE request_id = p_request_id;

        v_new_cert_id := cert_seq.NEXTVAL;

        INSERT INTO Certificates(certificate_id, certificate_code, student_id, type_id,
                                 title, issue_date, issuing_authority, status)
        VALUES (v_new_cert_id, p_code, v_student_id, v_type_id,
                v_title, NVL(v_pref_date, SYSDATE), v_authority, 'Valid');

        UPDATE Certificate_Requests
        SET status = 'Approved', reviewed_by = p_admin_id,
            reviewed_date = SYSDATE, certificate_id = v_new_cert_id
        WHERE request_id = p_request_id;
    END;

    PROCEDURE reject_request(p_request_id NUMBER, p_admin_id NUMBER) IS
    BEGIN
        UPDATE Certificate_Requests
        SET status = 'Rejected', reviewed_by = p_admin_id, reviewed_date = SYSDATE
        WHERE request_id = p_request_id;
    END;

    PROCEDURE get_dashboard_stats(p_total_students OUT NUMBER, p_total_certs OUT NUMBER,
                                  p_expired_certs OUT NUMBER, p_pending_requests OUT NUMBER) IS
    BEGIN
        SELECT COUNT(*) INTO p_total_students FROM Students;
        SELECT COUNT(*) INTO p_total_certs    FROM Certificates;
        SELECT COUNT(*) INTO p_expired_certs  FROM Certificates
            WHERE expiry_date IS NOT NULL AND expiry_date < SYSDATE;
        SELECT COUNT(*) INTO p_pending_requests FROM Certificate_Requests
            WHERE status = 'Pending';
    END;

END cert_pkg;

/

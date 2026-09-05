CREATE USER truelogs WITH PASSWORD 'truelogs42';
GRANT CONNECT ON DATABASE truelogs TO truelogs;
GRANT ALL PRIVILEGES ON DATABASE truelogs TO truelogs;
GRANT USAGE ON SCHEMA public TO truelogs;
GRANT SELECT ON public.logs TO truelogs;
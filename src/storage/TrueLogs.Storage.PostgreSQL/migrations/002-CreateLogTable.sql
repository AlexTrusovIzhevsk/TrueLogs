CREATE TABLE IF NOT EXISTS logs (
    id UUID PRIMARY KEY,
    timestamp TIMESTAMPTZ NOT NULL,
    level INTEGER NOT NULL,
    messagetemplate TEXT NOT NULL,
    renderedmessage TEXT NOT NULL,
    properties JSONB NOT NULL,
    exception TEXT,
    source TEXT NOT NULL,
    environment TEXT,
    traceid TEXT,
    spanid TEXT,
    servicetimestamp TIMESTAMPTZ NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_logs_timestamp ON logs(timestamp DESC);
CREATE INDEX IF NOT EXISTS idx_logs_level ON logs(level);
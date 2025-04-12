// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.
import * as initSqlJs from './sql-wasm.js';

let SQL = await initSqlJs.default({
    locateFile: file => `./_content/WebSql/${file}`
});
export function createDatabase() {
    return new SQL.Database();
}
export async function createDatabaseFromFile(sqliteDbLocation) {
    const fetchResult = await fetch(sqliteDbLocation);
    const buf = await fetchResult.arrayBuffer();
    return new SQL.Database(new Uint8Array(buf));
}
export async function exec(db, sql) {
    const buf = await db.exec(sql);
    console.log(buf);
    return buf;
}

import * as yargs from 'yargs';
import {Argv} from 'yargs';

let argv = yargs
    .command('update', 'update json content', (yargs: Argv) => {
        return yargs.option('file', {
            describe: 'file to update'
        });
    }).argv;

console.log(argv);

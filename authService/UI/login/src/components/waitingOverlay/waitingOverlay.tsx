import React from "react";
import {DotLoader} from "react-spinners";
import './waitingOverlay.scss';

let WaitingOverlay = (props: any) => (
    <div className="waiting-overlay-component">
        <div className="vert-center">
            <div className="horz-center">
                <DotLoader></DotLoader>
            </div>
        </div>
    </div>
);

export default WaitingOverlay;

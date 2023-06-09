import React from 'react'

import docriderStyles from '../styles/docrider.css'
import DocinsDropdownMenu from './DocinsDropdownMenu'

class DocumentControls extends React.Component {
    render() {
        return (
            <div className={docriderStyles.controlsContainer}>
                <div className={docriderStyles.btnGroup} title="New Narrative">
                    <button type="button">
                        <i className="fas fa-file-alt" />
                    </button>
                    <button
                        type="button"
                        onClick={this.props.onSaveClicked}
                        title="Save Narrative"
                    >
                        <i className="fas fa-save" />
                    </button>
                    <button type="button" title="Open Narrative">
                        <i className="fas fa-folder-open" />
                    </button>
                </div>
                <DocinsDropdownMenu />
            </div>
        )
    }
}

export default DocumentControls

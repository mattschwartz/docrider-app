import React from 'react'
import { connect } from 'react-redux'
import CharactersDocin from '../components/docins/CharactersDocin'
import OutlineDocin from '../components/docins/OutlineDocin'

import docinsStyles from '../styles/docins/docins.css'

class DocinsContainer extends React.Component {
    render() {
        const {
            isDocinsContainerVisible,
            charactersDocin,
            outlineDocin
        } = this.props

        return (
            isDocinsContainerVisible && (
                <div className={docinsStyles.docins}>
                    <OutlineDocin isVisible={outlineDocin.isVisible} />
                    <CharactersDocin isVisible={charactersDocin.isVisible} />
                </div>
            )
        )
    }
}

export default connect(state => ({
    isDocinsContainerVisible: state.docins.isDocinsContainerVisible,
    charactersDocin: state.docins.charactersDocin,
    outlineDocin: state.docins.outlineDocin
}))(DocinsContainer)

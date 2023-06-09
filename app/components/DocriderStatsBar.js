import React from 'react'

import styles from '../styles/home.css'

class DocriderStatsBar extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            filepath: 'Worlds',
            currentAct: 'Act 1',
            currentScene: 'Intro__EvelynnAndJames',
            isEdited: false,
            isEditing: false
        }
    }

    render() {
        return (
            <div className={styles.docstats}>
                {this.renderNarrativeStat()}
                {this.state.isEdited && <span> *</span>}
                {this.state.isEditing && <span> [editing]</span>}
                <span className={styles.fileStats}>
                    {this.renderActStat()} {this.renderSceneStat()}
                </span>
            </div>
        )
    }

    renderNarrativeStat() {
        return (
            <>
                <span className={styles.narrativeStat} title="Switch Narrative">
                    {this.state.filepath}
                </span>
            </>
        )
    }

    renderActStat() {
        return (
            <>
                <span className={styles.actStat} title="Switch Act">
                    Act: {this.state.currentAct}
                </span>
            </>
        )
    }

    renderSceneStat() {
        return (
            <>
                <span className={styles.sceneStat} title="Switch Scene">
                    Scene: {this.state.currentScene}
                </span>
            </>
        )
    }
}

export default DocriderStatsBar

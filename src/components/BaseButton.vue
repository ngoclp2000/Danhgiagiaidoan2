/<template>
    <div class="m-btn-container">
        <button :disabled="(isReadyData != null && !isReadyData) || disable" ref="button" :tabindex="tabindex"
            @keydown.enter="$emit('keydown.enter')" @click="$emit('click')" :id="id" :class="[classList]" :style="{backgroundImage: (hasIcon ? 'url(' + require('@/assets/icon/'+urlIcon) + ')': '')
      ,
      backgroundColor : (isHover ? bgHoverColor : bgColor),
      color : textColor,
      }" @mouseover="isHover = true" @mouseleave="isHover = false" v-tooltip="tooltipContent">
            <div class="btn-name">{{btnText}}</div>

        </button>
    </div>

</template>

<script>
    export default {
        props: ['hasIcon', 'tooltipContent', 'urlIcon', 'bgColor', 'classList', 'disable', 'btnText', 'id', 'textColor',
            'tabindex', 'isReadyData', 'bgHoverColor'
        ],
        data() {
            return {
                isHover: false,
            }
        }
    }
</script>

<style scoped>


</style>